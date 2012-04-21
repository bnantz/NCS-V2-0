/**
Copyright (c) 2005, Nantz Consulting & Software 

- All rights reserved. -

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
Redistributions in binary form must produce the above copyright notice, this list of conditions and the following disclaimer 
in the documentation and/or other materials provided with the distribution. Neither the name of Cornerstone Consulting nor 
the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
OF SUCH DAMAGE.
**/

//based on http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspp/html/ASPNETProvMod_Prt3.asp

using System;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Data.Common;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;

public class DbSiteMapProvider : StaticSiteMapProvider
{
    private const string _errmsg1 = "Missing node ID";
    private const string _errmsg2 = "Duplicate node ID";
    private const string _errmsg3 = "Missing parent ID";
    private const string _errmsg4 = "Invalid parent ID";
    private const string _errmsg5 =
        "Empty or missing connectionStringName";
    private const string _errmsg6 = "Missing connection string";
    private const string _errmsg7 = "Empty connection string";

    private int _indexID, _indexTitle, _indexUrl,
        _indexDesc, _indexRoles, _indexParent;
    private Dictionary<int, SiteMapNode> _nodes =
        new Dictionary<int, SiteMapNode>(16);
    private SiteMapNode _root;
    private bool _isParameterizedSql;

    public override SiteMapNode BuildSiteMap()
    {
        lock (this)
        {
            // Return immediately if this method has been called before
            if (_root != null)
                return _root;

            // Query the database for site map nodes
            try
            {
                //_isParameterizedSql = 

                Database database = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand;
                if(_isParameterizedSql)
                {
                    dbCommand = database.GetSqlStringCommand("SELECT [ID], [Title],[Description], [Url], [Roles], [Parent] FROM [SiteMap] ORDER BY [ID]");
                }
                else
                {
                    dbCommand = database.GetSqlStringCommand("proc_GetSiteMap");
                    dbCommand.CommandType = CommandType.StoredProcedure;
                }

                using (IDataReader dataReader = database.ExecuteReader(dbCommand))
                {
                    _indexID = dataReader.GetOrdinal("ID");
                    _indexUrl = dataReader.GetOrdinal("Url");
                    _indexTitle = dataReader.GetOrdinal("Title");
                    _indexDesc = dataReader.GetOrdinal("Description");
                    _indexRoles = dataReader.GetOrdinal("Roles");
                    _indexParent = dataReader.GetOrdinal("Parent");

                    if(dataReader.Read())
                    {
                        _root = CreateSiteMapNodeFromDataReader((DbDataReader)dataReader);
                        AddNode(_root, null);

                        // Build a tree of SiteMapNodes underneath
                        // the root node
                        while (dataReader.Read())
                        {
                            // Create another site map node and
                            // add it to the site map
                            SiteMapNode node =
                                CreateSiteMapNodeFromDataReader((DbDataReader)dataReader);
                            AddNode(node,
                                GetParentNodeFromDataReader((DbDataReader)dataReader));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Write(ex.ToString());
            }

            // Return the root SiteMapNode
            return _root;
        }
    }

    protected override SiteMapNode GetRootNodeCore()
    {
        BuildSiteMap();
        return _root;
    }

    // Helper methods
    private SiteMapNode
        CreateSiteMapNodeFromDataReader(DbDataReader reader)
    {
        // Make sure the node ID is present
        if (reader.IsDBNull(_indexID))
            throw new ProviderException(_errmsg1);

        // Get the node ID from the DataReader
        int id = reader.GetInt32(_indexID);

        // Make sure the node ID is unique
        if (_nodes.ContainsKey(id))
            throw new ProviderException(_errmsg2);

        // Get title, URL, description, and roles from the DataReader
        string title = reader.IsDBNull(_indexTitle) ?
            null : reader.GetString(_indexTitle).Trim();
        string url = reader.IsDBNull(_indexUrl) ?
            null : reader.GetString(_indexUrl).Trim();
        string description = reader.IsDBNull(_indexDesc) ?
            null : reader.GetString(_indexDesc).Trim();
        string roles = reader.IsDBNull(_indexRoles) ?
            null : reader.GetString(_indexRoles).Trim();

        // If roles were specified, turn the list into a string array
        string[] rolelist = null;
        if (!String.IsNullOrEmpty(roles))
            rolelist = roles.Split(new char[] { ',', ';' }, 512);

        // Create a SiteMapNode
        SiteMapNode node = new SiteMapNode(this, id.ToString(), url,
            title, description, rolelist, null, null, null);

        // Record the node in the _nodes dictionary
        _nodes.Add(id, node);

        // Return the node        
        return node;
    }

    private SiteMapNode
        GetParentNodeFromDataReader(DbDataReader reader)
    {
        // Make sure the parent ID is present
        if (reader.IsDBNull(_indexParent))
            throw new ProviderException(_errmsg3);

        // Get the parent ID from the DataReader
        int pid = reader.GetInt32(_indexParent);

        // Make sure the parent ID is valid
        if (!_nodes.ContainsKey(pid))
            throw new ProviderException(_errmsg4);

        // Return the parent SiteMapNode
        return _nodes[pid];
    }
}

