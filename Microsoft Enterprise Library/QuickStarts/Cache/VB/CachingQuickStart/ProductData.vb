'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Caching Application Block
'===============================================================================
' Copyright � 2004 Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

' ProductData allows the user interface to communicate
' with the DataProvider object. It also uses a 
' cache in order to prevent retrieving data from the
' DataProvider as much as possible.
Public Class ProductData
    Private cache As CacheManager
    Private dataProvider As DataProvider
    Public dataSourceMessage As String

    Public Sub New()
        ' Since no name is being used here, CacheFactory will return the
        ' object corresponding to the default CacheManager name according
        ' to the config file generated by ConfigurationConsole.exe.
        cache = CacheFactory.GetCacheManager("Loading Scenario Cache Manager")
        dataProvider = New DataProvider
    End Sub

    ' Retrieve an item -in this case, one product, by using an unique object identifier
    ' as key.
    Public Function ReadProductByID(ByVal productID As String) As Product

        Dim product As Product = DirectCast(cache.GetData(productID), Product)

        ' Does our cache already have the requested object?
        If (product Is Nothing) Then

            ' Requested object is not cached, so let's retrieve it from
            ' data provider and cache it for further requests.
            product = Me.dataProvider.GetProductByID(productID)

            If (Not product Is Nothing) Then
                cache.Add(productID, product)
                dataSourceMessage = String.Format(My.Resources.Culture, My.Resources.MasterSourceMessage, product.ProductID, product.ProductName, product.ProductPrice) & Environment.NewLine
            Else

                dataSourceMessage = My.Resources.ItemNotAvailableMessage & Environment.NewLine
            End If
        Else
            dataSourceMessage = String.Format(My.Resources.Culture, My.Resources.CacheSourceMessage, product.ProductID, product.ProductName, product.ProductPrice) & Environment.NewLine
        End If
        Return product
    End Function

    ' Reads all available items, adding them to the cache.
    Public Sub LoadAllProducts()
        Dim list As List(Of Product) = Me.dataProvider.GetProductList()
        Dim i As Integer
        For i = 0 To list.Count - 1
            Dim product As Product = list(i)
            cache.Add(product.ProductID, product)
        Next
    End Sub

    ' Removes all items from the cache..
    Public Sub FlushCache()
        cache.Flush()
    End Sub


End Class
