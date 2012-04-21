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

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NCS.Windows.Forms
{ 
  #region BaseValidator
  public abstract class BaseValidator : System.ComponentModel.Component {
  
    private Control _controlToValidate = null;
    private string _errorMessage = "";
    private Icon _icon = new Icon(typeof(ErrorProvider), "Error.ico");
    private static ErrorProvider _errorProvider = new ErrorProvider();
    private bool _isValid = false;
    
    [Category("Appearance")]
    [Description("Gets or sets the text for the error message.")]
    [DefaultValue("")]
    public string ErrorMessage {
      get { return _errorMessage; }
      set { _errorMessage = value; }
    }

    [Category("Appearance")]
    [Description("Gets or sets the Icon to display ErrorMessage.")]
    public Icon Icon {
      get { return _icon; }
      set { _icon = value; }
    }

    [Category("Behaviour")]
    [Description("Gets or sets the input control to validate.")]
    [DefaultValue(null)]
    [TypeConverter(typeof(ValidatableControlConverter))]
    public Control ControlToValidate {
      get { return _controlToValidate; }
      set {
      
        _controlToValidate = value;
      
        // Hook up ControlToValidate’s Validating event
        // at run-time ie not from VS.NET
        if( (_controlToValidate != null) && (!DesignMode) ) {
          _controlToValidate.Validating += 
            new CancelEventHandler(ControlToValidate_Validating);
        }
      }
    }
    
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsValid {
      get { return _isValid; }
      set { _isValid = value; }
    }  
    
    protected abstract bool EvaluateIsValid();
    
    public void Validate() {
      // Validate control
      _isValid = EvaluateIsValid();

      // Display an error if ControlToValidate is invalid, using 
      // the static ErrorProvider instance
      string errorMessage = "";
      if( !_isValid ) {
        errorMessage = _errorMessage;
        _errorProvider.Icon = _icon;
      }
      _errorProvider.SetError(_controlToValidate, errorMessage);
    }
    
    private void ControlToValidate_Validating(object sender, CancelEventArgs e) {
      // We don't cancel if invalid since we don't want to force
      // the focus to remain on ControlToValidate if invalid
      Validate();
    }
  }
  #endregion

  #region ValidationDataType
  public enum ValidationDataType
  {
      Currency,
      Date,
      Double,
      Integer,
      String
  }
  #endregion

  #region ValidationCompareOperator
  public enum ValidationCompareOperator
  {
      DataTypeCheck,
      Equal,
      GreaterThan,
      GreaterThanEqual,
      LessThan,
      LessThanEqual,
      NotEqual
  }
  #endregion

  #region ValidatableControlConverter
  public class ValidatableControlConverter : ReferenceConverter
  {
      public ValidatableControlConverter(Type type) : base(type) { }
      protected override bool IsValueAllowed(ITypeDescriptorContext context, object value)
      {
          return ((value is TextBox) ||
                  (value is ListBox) ||
                  (value is ComboBox) ||
                  (value is UserControl));
      }
  }
  #endregion

  #region RequiredFieldValidator
  [ToolboxBitmap(typeof(RequiredFieldValidator), "RequiredFieldValidator.ico")]
	public class RequiredFieldValidator : BaseValidator {
	
	  private string _initialValue = null;
	
    [Category("Behaviour")]
    [DefaultValue(null)]
    [Description("Sets or returns the base value for the validator. The default value is null.")]
    public string InitialValue {
      get { return _initialValue; }
      set { _initialValue = value; }
    }

	  protected override bool EvaluateIsValid() {
      string controlValue = ControlToValidate.Text.Trim();
      string initialValue;
      if( _initialValue == null ) initialValue = "";
      else initialValue = _initialValue.Trim();
      return (controlValue != initialValue);
	  }
	}
	#endregion
	
  #region RegularExpressionValidator
  [ToolboxBitmap(typeof(RegularExpressionValidator), "RegularExpressionValidator.ico")]
  public class RegularExpressionValidator : BaseValidator {
  
    private string _validationExpression = "";
    
    [Category("Behaviour")]
    [Description("Gets or sets the regular expression that determines the pattern used to validate a field.")]
    [DefaultValue("")]
    public string ValidationExpression {
      get { return _validationExpression; }
      set { _validationExpression = value; }
    }

    protected override bool EvaluateIsValid() {
      // Don't validate if empty
      if( ControlToValidate.Text.Trim() == "" ) return true;
      // Successful if match matches the entire text of ControlToValidate
      string field = ControlToValidate.Text.Trim();
      return Regex.IsMatch(field, _validationExpression.Trim());
    }
  }
  #endregion
  
  #region CustomValidator
  [ToolboxBitmap(typeof(CustomValidator), "CustomValidator.ico")]
  [DefaultEvent("Validating")]
  public class CustomValidator : BaseValidator {
    public class ValidatingCancelEventArgs {
      private bool _valid;
      private Control _controlToValidate;
      public ValidatingCancelEventArgs(bool valid, Control controlToValidate) {
        _valid = valid;
        _controlToValidate = controlToValidate;
      }
      public bool Valid {
        get { return _valid; }
        set { _valid = value; }
      }
      public Control ControlToValidate {
        get { return _controlToValidate; }
        set { _controlToValidate = value; }
      }
    }
    public delegate void ValidatingEventHandler(object sender, ValidatingCancelEventArgs e);
    [Category("Action")]
    [Description("Occurs when the CustomValidator validates the value of the ControlToValidate property.")]
    public event ValidatingEventHandler Validating;
    public void OnValidating(ValidatingCancelEventArgs e) {
      if( Validating != null ) Validating(this, e);
    }

    protected override bool EvaluateIsValid() {
      // Pass validation processing to event handler and wait for response
      ValidatingCancelEventArgs args = new ValidatingCancelEventArgs(false, this.ControlToValidate);
      OnValidating(args);
      return args.Valid;
    }
  }
  
  #endregion
  
  #region BaseCompareValidator
  public abstract class BaseCompareValidator : BaseValidator { 
  
    private ValidationDataType   _type = ValidationDataType.String;
    private string[]             _typeTable = new string[5] {"System.Decimal", 
                                                             "System.DateTime",
                                                             "System.Double",
                                                             "System.Int32",
                                                             "System.String"};
                                                             
    [Category("Behaviour")]
    [Description("Sets or returns the data type that specifies how to interpret the values being compared.")]
    [DefaultValue(ValidationDataType.String)]
    public ValidationDataType Type {
      get { return _type; }
      set { _type = value; }
    }

    protected TypeConverter TypeConverter {
      get { return TypeDescriptor.GetConverter(System.Type.GetType(_typeTable[(int)_type])); }
    }
      
    protected bool CanConvert(string value) {
      try {
        TypeConverter   _converter = TypeDescriptor.GetConverter(System.Type.GetType(_typeTable[(int)_type]));
        _converter.ConvertFrom(value);
        return true;
      }
      catch { return false; }
    }

    protected string Format(string value) {
      // If currency
      if( _type == ValidationDataType.Currency ) {
          // Convert to decimal format ie remove currency formatting characters
        return Regex.Replace(value, "[$ .]", "");
      }    
      return value;
    }
  }
  #endregion

  #region RangeValidator
  [ToolboxBitmap(typeof(RangeValidator), "RangeValidator.ico")]
  public class RangeValidator : BaseCompareValidator {
  
    private string _minimumValue = "";
    private string _maximumValue = "";
    
    [Category("Behaviour")]
    [Description("Sets or returns the value of the control that you are validating, which must be greater than or equal to the value of this property. The default value is an empty string (\"\").")]
    [DefaultValue("")]
    public string MinimumValue {
      get { return _minimumValue; }
      set { _minimumValue = value; }
    }

    [Category("Behaviour")]
    [Description("Sets or returns the value of the control that you are validating, which must be less than or equal to the value of this property. The default value is an empty string (\"\").")]
    [DefaultValue("")]
    public string MaximumValue {
      get { return _maximumValue; }
      set { _maximumValue = value; }
    }

    protected override bool EvaluateIsValid() {
      // Don't validate if empty, unless required
      if( ControlToValidate.Text.Trim() == "" ) return true;

      // Validate and convert Minimum
      if( _minimumValue.Trim() == "" ) throw new Exception("MinimumValue must be provided.");
      string formattedMinimumValue = Format(_minimumValue.Trim());
      if( !CanConvert(formattedMinimumValue) ) throw new Exception("MinimumValue cannot be converted to the specified Type.");
      object minimum = TypeConverter.ConvertFrom(formattedMinimumValue);

      // Validate and convert Maximum
      if( _maximumValue.Trim() == "" ) throw new Exception("MaximumValue must be provided.");
      string formattedMaximumValue = Format(_maximumValue.Trim());
      if( !CanConvert(formattedMaximumValue) ) throw new Exception("MaximumValue cannot be converted to the specified Type.");
      object maximum = TypeConverter.ConvertFrom(formattedMaximumValue);

      // Check minimum <= maximum
      if( Comparer.Default.Compare(minimum, maximum) > 0 ) throw new Exception("MinimumValue cannot be greater than MaximumValue.");

      // Check and convert ControlToValue
      string formattedValue = Format(ControlToValidate.Text.Trim());
      if( !CanConvert(formattedValue) ) return false;
      object value = TypeConverter.ConvertFrom(formattedValue);

      // Validate value's range (minimum <= value <= maximum)
      return( (Comparer.Default.Compare(minimum, value) <= 0) && 
        (Comparer.Default.Compare(value, maximum) <= 0) );
    }
  }
  #endregion
  
  #region CompareValidator
  [ToolboxBitmap(typeof(CompareValidator), "CompareValidator.ico")]
  public class CompareValidator : BaseCompareValidator {
  
    private string _valueToCompare = "";
    private Control _controlToCompare = null;
    private ValidationCompareOperator _operator = ValidationCompareOperator.Equal;
    
    [TypeConverter(typeof(ValidatableControlConverter))]
    [Category("Behaviour")]
    [Description("Gets or sets the input control to compare with the input control being validated.")]
    [DefaultValue(null)]
    public Control ControlToCompare {
      get { return _controlToCompare; }
      set { _controlToCompare = value; }
    }
    
    [Category("Behaviour")]
    [Description("Gets or sets the comparison operation to perform.")]
    [DefaultValue(null)]
    public ValidationCompareOperator Operator {
      get { return _operator; }
      set { _operator = value; }
    }
    
    [Category("Behaviour")]
    [Description("Gets or sets a constant value to compare with the value entered by the user into the input control being validated.")]
    [DefaultValue("")]
    public string ValueToCompare {
      get { return _valueToCompare; }
      set { _valueToCompare = value; }
    }
    
    protected override bool EvaluateIsValid() {
      // Don't validate if empty, unless required
      if( ControlToValidate.Text.Trim() == "" ) return true;

      // Can't evaluate if missing ControlToCompare and ValueToCompare
      if( (_controlToCompare == null) && (_valueToCompare == "") ) throw new Exception("The ControlToCompare property cannot be blank.");

      // Validate and convert CompareFrom
      string formattedCompareFrom = Format(ControlToValidate.Text);
      bool canConvertFrom = CanConvert(formattedCompareFrom);
      if( canConvertFrom ) {
        if( _operator == ValidationCompareOperator.DataTypeCheck ) return canConvertFrom;
      }
      else return false;
      object compareFrom = TypeConverter.ConvertFrom(formattedCompareFrom);

      // Validate and convert CompareTo
      string formattedCompareTo = Format(((_controlToCompare != null) ? _controlToCompare.Text : _valueToCompare));
      if( !CanConvert(formattedCompareTo) ) throw new Exception("The value you are comparing to cannot be converted to the specified Type.");
      object compareTo = TypeConverter.ConvertFrom(formattedCompareTo);

      // Perform comparison eg ==, >, >=, <, <=, !=
      int result = Comparer.Default.Compare(compareFrom, compareTo);
      switch( _operator ) {
        case ValidationCompareOperator.Equal :
          return (result == 0);
        case ValidationCompareOperator.GreaterThan :
          return (result > 0);
        case ValidationCompareOperator.GreaterThanEqual :
          return (result >= 0);
        case ValidationCompareOperator.LessThan :
          return (result < 0);
        case ValidationCompareOperator.LessThanEqual :
          return (result <= 0);
        case ValidationCompareOperator.NotEqual :
          return ((result != 0));
        default :
          return false;
      }
    }
  }
  #endregion
}
