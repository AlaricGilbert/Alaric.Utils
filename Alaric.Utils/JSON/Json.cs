using System;
using System.Collections;

namespace Alaric.Utils.JSON
{
    /// <summary>
    /// A light-weigit medium json analysier.
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Phrase the specified json into a JsonNode.
        /// </summary>
        /// <returns>The phrase.</returns>
        /// <param name="json">The specified json.</param>
        public static JsonNode Phrase(string json)
        {
            int p = 0;
            JsonNode node = Phrase(json, ref p);
            if (p != json.Length)
                throw new Exception();
            return node;
        }

        private static JsonNode Phrase(string json, ref int pos)
        {
            switch (json[pos])
            {
                case '"':
                    string nodeName = GetStringInQuotation(json, ref pos);
                    JsonNode node = Phrase(json, ref pos);
                    node.Name = nodeName;
                    return node;
                case '{':
                    pos++;
                    JsonNode nodeC = new JsonNode();
                    int startPos = pos;
                    while (json[pos] != '}')
                    {
                        MoveUntil(json, ref pos, '"');
                        string name = GetStringInQuotation(json, ref pos);
                        MoveUntil(json, ref pos, ':');
                        pos++;
                        MoveUntil(json, ref pos, '"', '{', '[');
                        if (json[pos] == '"')
                            nodeC.AddChild(new JsonElement
                            {
                                FatherElement = nodeC,
                                Name = name,
                                Value = GetStringInQuotation(json, ref pos)
                            });
                        else
                        {
                            JsonNode nodeTmp = Phrase(json, ref pos);
                            nodeTmp.FatherElement = nodeC;
                            nodeTmp.Name = name;
                            nodeC.AddChild(nodeTmp);
                        }

                        MoveUntil(json, ref pos, ',', '}');
                        if (json[pos] == '}')
                        {
                            nodeC.Value = json.Substring(startPos, pos - startPos).Trim();
                            pos++;
                            return nodeC;
                        }

                        pos++;
                    }

                    return null;
                case '[':
                    pos++;
                    JsonNode nodeS = new JsonNode();
                    int startPosi = pos;
                    int index = 0;
                    nodeS.IsArray = true;
                    while (json[pos] != ']')
                    {
                        MoveUntil(json, ref pos, '"', '[', '{');
                        if (json[pos] == '"')
                        {
                            string value = GetStringInQuotation(json, ref pos);
                            nodeS.AddChild(new JsonElement
                            {
                                FatherElement = nodeS,
                                Name = $"element{index}",
                                IsArrayChild = true,
                                Value = value
                            });
                            pos++;
                        }
                        else
                        {
                            JsonNode nodeS_tmp = Phrase(json, ref pos);
                            nodeS_tmp.FatherElement = nodeS;
                            nodeS_tmp.Name = $"element{index}";
                            nodeS_tmp.IsArrayChild = true;
                            nodeS.AddChild(nodeS_tmp);
                        }

                        MoveUntil(json, ref pos, ',', ']');
                        if (json[pos] == ']')
                        {
                            nodeS.Value = json.Substring(startPosi, pos - startPosi).Trim();
                            pos++;
                            return nodeS;
                        }

                        pos++;
                        index++;
                    }

                    return null;
                default:
                    pos++;
                    return Phrase(json, ref pos);
            }
        }

        private static string GetStringInQuotation(string str, ref int pos)
        {
            //if the char of the current position is not a quotation, throws a exception.
            if (str[pos] != '"')
                throw new Exception();

            //move to next position.
            pos++;

            //record current position.
            int startIndex = pos;

            //find the position of the next quotation.
            while (str[pos] != '"')
                pos++;

            //get the string in the quotation.
            string result = str.Substring(startIndex, pos - startIndex);

            //move to next quotation
            pos++;

            return result;
        }

        private static void MoveUntil(string str, ref int pos, params char[] chars)
        {
            //remove blanks
            while (new ArrayList
            {
                ' ',
                '\r',
                '\n',
                '\t'
            }.Contains(str[pos]))
                pos++;

            //if str[pos] equals any item in chars, return 
            foreach (var c in chars)
                if (str[pos] == c)
                    return;

            //if no item in chars equals str[pos], throws a exception
            throw new Exception();
        }
    }
}