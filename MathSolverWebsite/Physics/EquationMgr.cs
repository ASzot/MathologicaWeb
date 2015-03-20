using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;

namespace MathSolverWebsite.Physics
{
    class EquationMgr
    {
        private List<PhysicsEqData> _physicsEqDatas;

        public List<WebsiteHelpers.TopicPath> PhysicsEqData
        {
            get { return _physicsEqDatas.Cast<WebsiteHelpers.TopicPath>().ToList(); }
        }

        public EquationMgr()
        {

        }

        public PhysicsEqData GetEqData(string eqName)
        {
            foreach (var physicsEqData in _physicsEqDatas)
            {
                if (physicsEqData.DispName == eqName)
                    return physicsEqData;
            }

            return null;
        }

        public bool LoadEquations(string xmlFilePath, bool useRad)
        {
            if (_physicsEqDatas != null && _physicsEqDatas.Count != 0)
                return true;


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            _physicsEqDatas = new List<PhysicsEqData>();


            XmlNodeList equations = xmlDoc.GetElementsByTagName("equation");
            foreach (XmlNode equationNode in equations)
            {
                string equationStr = null;
                string path = null;
                string dispName = null;
                string info = "";
                Dictionary<string, bool> variableListings = new Dictionary<string, bool>();
                List<string> unitListings = new List<string>();
                List<string> variableHints = new List<string>();

                foreach (XmlNode node in equationNode.ChildNodes)
                {
                    if (node.Name == "name")
                        dispName = node.InnerText;
                    else if (node.Name == "path")
                        path = node.InnerText;
                    else if (node.Name == "info")
                        info = node.InnerText;
                    else if (node.Name == "eq")
                        equationStr = node.InnerText;
                    else if (node.Name == "hint")
                        variableHints.Add(node.InnerText);
                    else if (node.Name == "var")
                    {
                        bool isFunc = false;
                        if (node.Attributes["isFunc"] != null)
                        {
                            string isFuncStr = node.Attributes["isFunc"].InnerText;
                            if (!bool.TryParse(isFuncStr, out isFunc))
                                return false;
                        }

                        variableListings.Add(node.InnerText, isFunc);

                        var unitNode = node.Attributes["unit"];
                        if (unitNode == null)
                            return false;

                        var hintNode = node.Attributes["hint"];
                        if (hintNode == null)
                            return false;

                        variableHints.Add(hintNode.InnerText);
                        unitListings.Add(unitNode.InnerText);
                    }
                    else
                        return false;
                }

                if (equationStr == null || path == null || dispName == null)
                    return false;

                PhysicsEqData ped = new PhysicsEqData(path, dispName, info);

                var evalData = new MathSolverLibrary.TermType.EvalData(useRad, new MathSolverLibrary.WorkMgr(), new MathSolverLibrary.Information_Helpers.FuncDefHelper());
                if (!ped.Init(equationStr, variableListings, variableHints, unitListings, ref evalData))
                    return false;

                _physicsEqDatas.Add(ped);
            }

            return true;
        }
    }
}