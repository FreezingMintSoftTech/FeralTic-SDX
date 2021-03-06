﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public static class ShaderCompilerErrorParser
    {
        private const string iGNORE_DEPRECATE = "warning X4717: Effects deprecated for D3DCompiler_47";

        public static CompilerResults ParseCompilerResult(string ErrorMessage, string shaderName = "")
        {
            CompilerResults compilerResults = new CompilerResults(null);

            string[] errors = ErrorMessage.Split("\n".ToCharArray());
            foreach (string s2 in errors)
            {
                if (!string.IsNullOrEmpty(s2))
                {
                    string s = s2;
                    if (s.Length > 0 && s != iGNORE_DEPRECATE)
                    {
                        var error = ParseLine(s,shaderName);
                        if (error != null)
                        {
                            compilerResults.Errors.Add(error);
                        }
                    }
                }
            }


            return compilerResults;
        }

        private static CompilerError ParseLine(string line, string shaderName = "")
        {
            CompilerError ce = new CompilerError();
            ce.ErrorText = "";

            var elements = line.Split(new string[1] { ": " }, StringSplitOptions.None);


            //First items contains filename + line/char
            var fileLine = elements[0].Split("(".ToCharArray());

            if (shaderName.Length == 0)
            {
                try
                {
                    ce.FileName = Path.GetFileName(fileLine[0]);
                }
                catch
                {
                    ce.FileName = fileLine[0];
                }
            }
            else
            {
                ce.FileName = shaderName;
            }


            try
            {
                var lineChar = fileLine[1].Replace(")", "").Split(",".ToCharArray());
                ce.Line = int.Parse(lineChar[0]);

                if (lineChar[1].Contains("-"))
                {
                    var startEnd = lineChar[1].Split('-');
                    ce.Column = int.Parse(startEnd[1]);
                }
                else
                {
                    ce.Column = int.Parse(lineChar[1]);
                }

                
            }
            catch
            {
                ce.Line = -1;
                ce.Column = -1;
            }

            if (elements.Length == 2)
            {
                //No error provided
                ce.ErrorNumber = "-1";
                ce.ErrorText = elements[1];
                ce.IsWarning = false;
            }
            else
            {
                try
                {
                    if (elements.Length == 3)
                    {
                        var errCode = elements[1].Split(" ".ToCharArray());
                        ce.IsWarning = errCode[0] == "warning";
                        ce.ErrorNumber = errCode[1];

                        ce.ErrorText = elements[2];
                    }
                }
                catch { }


            }
            

            return ce;
        }


    }
}
