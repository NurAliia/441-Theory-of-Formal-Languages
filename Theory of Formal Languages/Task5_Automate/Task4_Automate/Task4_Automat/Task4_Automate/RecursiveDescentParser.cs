using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Task4_Automate.SearchSubstring;

namespace Task4_Automate
{
    struct Result
    {
        bool isApplied;
        int lenght;
        Dictionary<int, List<int>> regulations;
        public Result(bool isApplied, int lenght, Dictionary<int, List<int>> regulations)
        {
            this.isApplied = isApplied;
            this.lenght = lenght;
            this.regulations = regulations;
        }
    }
    class RecursiveDescentParser
    {
        List<int> result = new List<int>();
        int l = 0;
        public List<int> RecursiveDescentParserMethod(ref int k ,List<MaxStringResult> listTokens)
        {
            while (k < listTokens.Count && listTokens[k].nameAutomate == "OpenBkt")
            {
                k++;
                while (k < listTokens.Count)
                {
                    switch (listTokens[k].maxString)
                    {
                        case "lambda":
                            {
                                k++;
                                if (FirstRule(ref k, listTokens))
                                {
                                    result.Add(1);
                                }
                                break;
                            }
                        case "if":
                            {
                                k++;
                                if (SecondRule(ref k, listTokens))
                                {
                                    result.Add(2);
                                }
                                break;
                            }
                        case "cond":
                            {
                                k++;
                                if (ThirdRule(ref k, listTokens))
                                {
                                    result.Add(3);
                                }
                                break;
                            }
                        case "define":
                            {
                                k++;
                                if (FourthRule(ref k, listTokens))
                                {
                                    result.Add(4);
                                }
                                break;
                            }
                        case "(":
                            {
                                k++;
                                break;
                            }                        
                        default:
                            {
                                return result;
                            }
                    }
                }
            }
            return result;
        }
        private bool FirstRule(ref int k, List<MaxStringResult> listTokens)
        {
            if (listTokens[k].nameAutomate == "OpenBkt")
            {
                k++;
                while (listTokens[k].nameAutomate == "Identify")
                {
                    k++;
                }
                if (listTokens[k].nameAutomate == "CloseBkt")
                {
                    k++;
                    bool res = true;
                    int iteration = 0;
                    while (res && iteration < 4)
                    {
                        int l = k;
                        RecursiveDescentParserMethod(ref k, listTokens);
                        if (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                        {
                            while (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                            {
                                k++;
                            }
                            iteration++;
                        }
                        else if (listTokens[k].nameAutomate == "Identify")
                        {
                            k++;
                            iteration++;
                        }                       
                        else if (k != l)
                        {
                            if (listTokens[k].nameAutomate != "CloseBkt")
                            {
                                k++;
                            }
                            iteration++;
                        }
                        else
                        {
                            res = false;
                        }
                    }
                    if (listTokens[k].nameAutomate == "CloseBkt")
                    {
                        k++;
                        if (iteration != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private bool SecondRule(ref int k, List<MaxStringResult> listTokens)
        {
            bool res = true;
            int iteration = 0;           
            while (res && iteration < 3)
            {
                int l = k;
                RecursiveDescentParserMethod(ref k, listTokens);
                if (k != l && listTokens[k].nameAutomate != "CloseBkt")
                {
                    k++;
                    iteration++;
                }
                else if (listTokens[k].nameAutomate == "Identify")
                {
                    while (listTokens[k].nameAutomate == "Identify")
                    {
                        k++;
                    }
                    iteration++;
                }
                else
                {
                    res = false;
                }
            }        
            if (listTokens[k].nameAutomate == "CloseBkt")
            {
                k++;
                if (iteration != 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ThirdRule(ref int k, List<MaxStringResult> listTokens)
        {
            bool res = true;
            int iteration = 0;
            while (res && iteration < 3)
            {
                if (listTokens[k].nameAutomate == "OpenBkt")
                {
                    int l = k;
                    RecursiveDescentParserMethod(ref k, listTokens);
                    if (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                    {
                        while (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                        {
                            k++;
                        }
                    }
                    else if (listTokens[k].nameAutomate == "Identify")
                    {
                        while (listTokens[k].nameAutomate == "Identify")
                        {
                            k++;
                        }
                    }                   
                    else if (k != l && listTokens[k].nameAutomate != "CloseBkt")
                    {
                        k++;
                        iteration++;
                    }
                    if (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                    {
                        while (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                        {
                            k++;
                        }
                    }
                    else if (listTokens[k].nameAutomate == "Identify")
                    {
                        while (listTokens[k].nameAutomate == "Identify")
                        {
                            k++;
                        }
                    }
                    else if (RecursiveDescentParserMethod(ref k, listTokens).Count == 0)
                    {
                        k++;
                    }
                    if (listTokens[k].nameAutomate == "CloseBkt")
                    {
                        k++;
                    }
                }
                else
                {
                    if (iteration != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        private bool FourthRule(ref int k, List<MaxStringResult> listTokens)
        {
            if (listTokens[k].nameAutomate == "Identify")
            {
                k++;
                if (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                {
                    while (listTokens[k].nameAutomate == "Int" || listTokens[k].nameAutomate == "Real")
                    {
                        k++;
                    }
                }
                else if (listTokens[k].nameAutomate == "Identify")
                {
                    k++;
                }
                else if (RecursiveDescentParserMethod(ref k, listTokens).Count == 0)                      
                {
                    k++;
                }
                if (listTokens[k].nameAutomate == "CloseBkt")
                {
                    k++;
                    return true;
                }
            }
            return false;
        }
    }
}
