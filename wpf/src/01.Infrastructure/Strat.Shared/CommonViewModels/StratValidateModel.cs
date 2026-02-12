using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Strat.Shared.CommonViewModels
{
    public class StratValidateModel : BindableBase, IDataErrorInfo
    {
        private bool _validationPerformed;
        private readonly Dictionary<string, string> _dataErrors = new();
        
        public bool Validate()
        {
            _dataErrors.Clear();
            _validationPerformed = true;
            
            var vc = new ValidationContext(this);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, vc, results, true);

            // Populate _dataErrors so binding engine can read them if needed
            foreach (var result in results)
            {
                if (result.MemberNames.Any())
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        if (!_dataErrors.ContainsKey(memberName))
                        {
                            _dataErrors[memberName] = result.ErrorMessage ?? "";
                        }
                    }
                }
            }

            // Notify UI to update validation state
            Type type = GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var methodInfo = property.GetGetMethod();
                if (methodInfo != null && methodInfo.IsVirtual)
                {
                    RaisePropertyChanged(property.Name);
                }
            }
            
            return isValid;
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                var property = GetType().GetProperty(columnName);
                if (property == null) return string.Empty;

                var vc = new ValidationContext(this, null, null)
                {
                    MemberName = columnName
                };
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(property.GetValue(this, null), vc, res);
                if (res.Count > 0 && _validationPerformed)
                {
                    var errorInfo = string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                    if (!_dataErrors.ContainsKey(columnName))
                    {
                        _dataErrors.Add(columnName, errorInfo);
                    }
                    return errorInfo;
                }
                return string.Empty;
            }
        }
    }
}

