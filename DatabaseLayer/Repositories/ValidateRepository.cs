using System;
using System.Text.RegularExpressions;
using Type = DatabaseLayer.Models.Type;

namespace DatabaseLayer.Repositories
{
    public class ValidateRepository
    {
        public bool Validate(Type type, string data)
        {
            string patternCharInvl = @"^\s*\(\s*[a-z]\s*[,;]\s*[a-z]\s*\)\s*$";
            
            bool typeOK = false;
            switch (type)
            {
                case Type.Integer:
                {
                    typeOK=Int32.TryParse(data, out _);
                    break;
                }
                case Type.Real:
                {
                    double d;
                    typeOK = Double.TryParse(data, out d);
                    break;
                }
                case Type.Char:
                {
                    char ch;
                    typeOK = Char.TryParse(data, out ch);
                    break;
                }
                case Type.String:
                    typeOK = true;
                    break;
                case Type.CharInvl:
                {
                    typeOK = Regex.IsMatch(data, patternCharInvl, RegexOptions.IgnoreCase);
                    break;
                }
                case Type.String_charInvl:
                {
                    var separates = data.Split(' ');
                    foreach (var charInvl in separates)
                    {
                        typeOK = Regex.IsMatch(charInvl, patternCharInvl, RegexOptions.IgnoreCase);
                        if(!typeOK) break;
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return typeOK;
        }
    }
}