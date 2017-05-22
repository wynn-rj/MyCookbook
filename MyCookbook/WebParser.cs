using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MyCookbook
{
    static class WebParser
    {
        private static char[] TRIM_CHARS = { '\n', '\t', ' ', '\r', '\f' };
        public const char SEPERATOR = '|';
        public const string STEP_SEPERATOR = "||STEPS||";


        public static string[] ParseSite(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = null;
            try
            {
                doc = web.Load(url);
            }
            catch (Exception e)
            {
                if (e.Message.Equals(@"'gzip' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.
Parameter name: name"))
                {
                    string html;
                    using (WebClient wc = new GZipWebClient())
                    {
                        html = wc.DownloadString(url);
                    }

                    doc = new HtmlDocument();
                    doc.LoadHtml(html);
                }
                else
                {
                    throw e;
                }

            }

            string[] parsedSiteInfo;

            if (url.ToLower().Contains("bettycrocker"))
            {
                parsedSiteInfo = ParseBettyCrocker(doc);
            }
            else if (url.ToLower().Contains("allrecipes"))
            {
                parsedSiteInfo = ParseAllrecipes(doc);
            }
            else if (url.ToLower().Contains("seriouseats"))
            {
                parsedSiteInfo = ParseSeriousEats(doc);
            }
            else if (url.ToLower().Contains("food.com"))
            {
                parsedSiteInfo = ParseFoodCom(doc, url.Substring(url.LastIndexOf('-') + 1));
            }
            else if (url.ToLower().Contains("foodnetwork"))
            {
                parsedSiteInfo = ParseFoodNetwork(doc);
            }
            else if (url.ToLower().Contains("epicurious"))
            {
                parsedSiteInfo = ParseEpicurious(doc);
            }
            else if (url.ToLower().Contains("myrecipes"))
            {
                parsedSiteInfo = ParseMyrecipes(doc);
            }
            else if (url.ToLower().Contains("centercutcook"))
            {
                parsedSiteInfo = ParseCenterCutCook(doc);
            }
            else if (url.ToLower().Contains("simplyrecipes"))
            {
                parsedSiteInfo = ParseSimplyRecipes(doc);
            }
            else if (url.ToLower().Contains("wilton"))
            {
                parsedSiteInfo = ParseWilton(doc);
            }
            else if (url.ToLower().Contains("macheesmo"))
            {
                parsedSiteInfo = ParseMacheesmo(doc);
            }
            else if (url.ToLower().Contains("cooksillustrated"))
            {
                WebPagePasswordRequestDialog usrPswdDialog = new WebPagePasswordRequestDialog();
                usrPswdDialog.ShowDialog();
                string username = usrPswdDialog.usernameTextBox.Text;
                string password = usrPswdDialog.passwordTextBox.Text;
                parsedSiteInfo = ParseCooksIllustrated(url, username, password);
            }
            else
            {
                parsedSiteInfo = new string[] { "not able to parse site" };
            }

            return parsedSiteInfo;

        }

        private static string[] ParseBettyCrocker(HtmlDocument doc)
        {
            string recipeNameNode = @"//*[@id=""gmi_rp_recipeTitle""]";
            string imageNode = @"//*[@id=""gmi_rp_recipeImage""]";
            string ingredientNumberNode = @"//*[@id=""gmi_rp_recipeIngredients_parts_{0}""]/dt/span";
            string ingredientTextNode = @"//*[@id=""gmi_rp_recipeIngredients_parts_{0}""]/dd/span[1]";
            string stepsNode = @"//*[@id=""gmi_rp_recipeSteps_step_{0}""]/div/div[2]";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].InnerHtml;

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img.Trim(TRIM_CHARS).Split('"')[1]);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection numberNode = doc.DocumentNode.SelectNodes(String.Format(ingredientNumberNode, index));
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (numberNode != null)
                {
                    for (int i = 0; i < numberNode.Count; i++)
                    {
                        string ing = numberNode[i].InnerText.Trim(TRIM_CHARS) + SEPERATOR + textNode[i].InnerText.Trim(TRIM_CHARS);
                        ingredients.Add(ing);
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    ingredients.Add(step);
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseAllrecipes(HtmlDocument doc)
        {
            string recipeNameNode = @"/html/body/div[1]/div[2]/div/div[3]/section/div[1]/div/section[2]/h1";
            string imageNode = @"/html/body/div[1]/div[2]/div/div[3]/section/div[1]/div/section[1]/span/a[1]";
            string ingredientTextNode = @"//*[@id=""lst_ingredients_{0}""]/li[{1}]/label/span";
            string stepsNode = @"/html/body/div[1]/div[2]/div/div[3]/section/section[2]/div/div[1]/ol/li[{0}]/span";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].InnerHtml;

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img.Trim(TRIM_CHARS).Split('"')[3]);


            index = 1;
            int colIndex = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, colIndex, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS);
                        if (!ing.Equals("Add all ingredients to list"))
                        {
                            if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                            {
                                ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1));
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }
                        }
                    }
                    index++;
                }
                else
                {
                    if (index == 1)
                    {
                        isIngredients = false;
                    }
                    else
                    {
                        colIndex++;
                        index = 1;
                    }
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    ingredients.Add(step);
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseSeriousEats(HtmlDocument doc)
        {
            string recipeNameNode = @"/html/body/div[2]/section[1]/article/header/div/h1";
            string imageNode = @"/html/body/div[2]/section[1]/article/header/figure/div[2]";
            string ingredientTextNode = @"/html/body/div[2]/section[1]/article/div/div[3]/div[1]/ul/li[{0}]";
            string stepsNode = @"/html/body/div[2]/section[1]/article/div/div[3]/div[2]/ol/li[{0}]/div[2]/p[1]";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].InnerHtml;

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img.Trim(TRIM_CHARS).Split('"')[1]);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS);
                        if (!ing.Equals("Add all ingredients to list"))
                        {
                            if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                            {
                                ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1));
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }
                        }
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    ingredients.Add(step);
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseFoodCom(HtmlDocument doc, string id)
        {
            string recipeNameNode = @"//*[@id=""" + id + @"""]/div[1]/div[2]/header/h1";
            string imageNode = @"//*[@id=""" + id + @"""]/section[1]/div[1]/div";
            string ingredientTextNode = @"//*[@id=""" + id + @"""]/section[2]/div/div[2]/ul/li[{0}]";
            string stepsNode = @"//*[@id=""" + id + @"""]/section[2]/div/div[4]/ol/li[{0}]";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].InnerHtml.Split(new string[] { "/convert" }, StringSplitOptions.RemoveEmptyEntries)[0];

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img.Trim(TRIM_CHARS).Split('"')[13]);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS).Replace('\u2044', '/').Replace("&frasl;", "/");
                        if (!ing.Equals(""))
                        {
                            if (ing.IndexOf(' ') != -1)
                            {
                                if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                                {
                                    ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                                }
                                else
                                {
                                    ingredients.Add(ing);
                                }
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }

                        }
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals("") && !step.Equals("Submit a Correction"))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseFoodNetwork(HtmlDocument doc)
        {
            string recipeNameNode = "/html/head/title";
            string imageNode = "/html/head/meta";
            string ingredientAndDirectionTextNode = "/html/head/script";
            int index = -1;

            List<string> ingredients = new List<string>();

            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[11].GetAttributeValue("content", "");

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);

            HtmlNodeCollection scripts = doc.DocumentNode.SelectNodes(ingredientAndDirectionTextNode);
            string scriptWithRecipeInfo = "";
            foreach (HtmlNode h in scripts)
            {
                if (h.InnerText.Contains("recipeIngredient"))
                {
                    scriptWithRecipeInfo = h.InnerText;
                }
            }

            string[] splitScript = scriptWithRecipeInfo.Trim(' ', '{', '}').Split('[', ']');

            int j = 0;
            foreach (string s in splitScript)
            {
                s.Trim(' ', '\n', '\"');
                if (s.Contains("recipeIngredient"))
                {
                    index = j;
                }
                j++;
            }
            string[] ing = splitScript[index + 1].Split(new string[] { "\"," }, StringSplitOptions.RemoveEmptyEntries);
            string[] instructions = splitScript[index + 3].Split(new string[] { "\"," }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ing.Length; i++)
            {
                string s = ing[i].Trim(TRIM_CHARS).Trim('\"');
                if (!s.Equals(""))
                {
                    if (s.IndexOf(' ') != -1)
                    {
                        if (IsDigitOrSlashOnly(s.Substring(0, s.IndexOf(' '))))
                        {
                            ingredients.Add(s.Substring(0, s.IndexOf(' ')) + SEPERATOR + s.Substring(s.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                        }
                        else
                        {
                            ingredients.Add(s);
                        }
                    }
                    else
                    {
                        ingredients.Add(s);
                    }

                }
            }

            for (int i = 0; i < instructions.Length; i++)
            {
                string s = instructions[i].Trim(TRIM_CHARS).Trim('\"');
                if (!s.Equals(""))
                {
                    ingredients.Add(s);
                }
            }

            return ingredients.ToArray();
        }

        private static string[] ParseEpicurious(HtmlDocument doc)
        {
            string recipeNameNode = @"/html/head/meta";
            string imageNode = @"/html/head/meta";
            string ingredientTextNode = @"/html[1]/body[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[4]/div[1]/div[2]/ol[1]/li[{0}]/ul/li[{1}]";
            string stepsNode = @"/html[1]/body[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[4]/div[1]/div[3]/ol[1]/li[{0}]";

            int index;

            List<string> ingredients = new List<string>();

            HtmlNode hx = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[4]/div[1]/div[3]")[0];


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[21].GetAttributeValue("content", "");
            string img = doc.DocumentNode.SelectNodes(imageNode)[26].GetAttributeValue("content", "");

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);


            index = 1;
            int colIndex = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, colIndex, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS);
                        if (!ing.Equals("Add all ingredients to list"))
                        {
                            if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                            {
                                ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1));
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }
                        }
                    }
                    index++;
                }
                else
                {
                    if (index == 1)
                    {
                        isIngredients = false;
                    }
                    else
                    {
                        colIndex++;
                        index = 1;
                    }
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals(""))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseMyrecipes(HtmlDocument doc)
        {
            string recipeNameNode = @"//*[@id=""block-system-main""]/div/div[2]/div/div[3]/div/div/div/h1";
            string imageNode = @"//*[@id=""block-system-main""]/div/div[3]/main/header/div[2]/div/div/div/figure/div/img";
            string ingredientTextNode = @"//*[@id=""block-system-main""]/div/div[3]/main/div[1]/div[1]/div[1]/div/div/ul/li[{0}]";
            string stepsNode = @"//*[@id=""block-system-main""]/div/div[3]/main/div[1]/div[2]/div[1]/div/div/ol/li[{0}]/div/p";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].GetAttributeValue("src", "").Split('?')[0];

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS);
                        if (!ing.Equals(""))
                        {
                            if (ing.IndexOf(' ') != -1)
                            {
                                if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                                {
                                    ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                                }
                                else
                                {
                                    ingredients.Add(ing);
                                }
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }

                        }
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals("") && !step.Equals("Submit a Correction"))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseCenterCutCook(HtmlDocument doc)
        {
            string recipeNameNode = @"//*[@id=""content""]/div[2]/div[1]/div[8]/h2/a";
            string imageNode = @"//*[@id=""content""]/div[2]/div[1]/div[8]/p/img";
            string ingredientTextNode = @"//*[@id=""content""]/div[2]/div[1]/div[8]/ul/li[{0}]";
            string stepsNode = @"//*[@id=""content""]/div[2]/div[1]/div[8]/ol/li[{0}]";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].GetAttributeValue("src", "").Split('?')[0];

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS);
                        if (!ing.Equals(""))
                        {
                            if (ing.IndexOf(' ') != -1)
                            {
                                if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                                {
                                    ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                                }
                                else
                                {
                                    ingredients.Add(ing);
                                }
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }

                        }
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals("") && !step.Equals("Submit a Correction"))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseSimplyRecipes(HtmlDocument doc)
        {
            string recipeNameNode = @"//*[@id=""content""]/article/header/h1";
            string imageNode = @"//*[@id=""content""]/article/div[1]/div[2]/img";
            string ingredientTextNode = @"//*[@id=""content""]/article/div[1]/div[8]/div[3]/ul/li[{0}]";
            string stepsNode = @"//*[@id=""content""]/article/div[1]/div[8]/div[5]/div/p[{0}]";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].GetAttributeValue("src", "").Split('?')[0];

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS);
                        if (!ing.Equals(""))
                        {
                            if (ing.IndexOf(' ') != -1)
                            {
                                if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                                {
                                    ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                                }
                                else
                                {
                                    ingredients.Add(ing);
                                }
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }

                        }
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals("") && !step.Equals("Submit a Correction"))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseWilton(HtmlDocument doc)
        {
            string recipeNameNode = @"//*[@id=""pdpMain""]/div[2]/h1";
            string imageNode = @"//*[@id=""pdpMain""]/div[1]/div[1]/img";
            string ingredientTextNode = @"//*[@id=""tab2""]/div[4]/div[{0}]/div[2]/h1";
            string stepsNode = @"//*[@id=""tab1""]/div[1]/div[{0}]/div/div/p";

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].GetAttributeValue("src", "").Split('?')[0];

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS).Replace('\n', ' ');
                        if (!ing.Equals(""))
                        {
                            if (ing.IndexOf(' ') != -1)
                            {
                                if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                                {
                                    ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                                }
                                else
                                {
                                    ingredients.Add(ing);
                                }
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }

                        }
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals("") && !step.Equals("Submit a Correction"))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseMacheesmo(HtmlDocument doc)
        {
            string recipeNameNode = @"//*[@id=""main""]/article/div/div[2]/h1";
            string imageNode = @"//*[@id=""main""]/article/header/div/img";
            string ingredientTextNode = @"//*[@id=""recipe-1""]/header/div[4]/div[{0}]";
            string stepsNode = @"//*[@id=""recipe-1""]/div/p[{0}]";


            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[0].GetAttributeValue("src", "").Split('?')[0];

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);


            index = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS);
                        if (!ing.Equals(""))
                        {
                            if (ing.IndexOf(' ') != -1)
                            {
                                if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                                {
                                    ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                                }
                                else
                                {
                                    ingredients.Add(ing);
                                }
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }

                        }
                    }
                    index++;
                }
                else
                {
                    isIngredients = false;
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals("") && !step.Equals("Submit a Correction"))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }

        private static string[] ParseCooksIllustrated(string url, string username, string password)
        {
            string recipeNameNode = @"/html/head/title";
            string imageNode = @"/html/head/meta";
            string ingredientTextNode = @"/html/body/div[3]/main/div[2]/div[2]/div/div[1]/div[2]/div[{0}]/table[{1}]/tbody/tr";
            string stepsNode = @"/html/body/div[3]/main/div[2]/div[2]/div/div[2]/div[2]/p[{0}]";

            var cookieJar = new CookieContainer();
            CookieAwareWebClient client = new CookieAwareWebClient(cookieJar);

            // the website sets some cookie that is needed for login, and as well the 'authenticity_token' is always different
            string response = client.DownloadString("https://www.cooksillustrated.com/sign_in");

            // parse the 'authenticity_token' and cookie is auto handled by the cookieContainer
            string token = Regex.Match(response, "authenticity_token.+?value=\"(.+?)\"").Groups[1].Value;
            NameValueCollection values = new NameValueCollection{
                    {"utf8", "✓" },
                    {"authenticity_token", token },
                    { "user[email]", username},
                    {"user[password]", password }
                };

            //string postData = string.Format("utf8=%E2%9C%93&authenticity_token={0}&user%5Bemail%5D=wynnpublic@gmail.com&user%5Bpassword%5D=caspian1", token);


            //WebClient.UploadValues is equivalent of Http url-encode type post
            client.Method = "POST";
            //response = client.UploadString("https://www.cooksillustrated.com/sessions", postData);
            client.UploadValues("https://www.cooksillustrated.com/sessions", "POST", values);

            string html = client.DownloadString(url);


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            int index;

            List<string> ingredients = new List<string>();


            string recipeName = doc.DocumentNode.SelectNodes(recipeNameNode)[0].InnerText;
            string img = doc.DocumentNode.SelectNodes(imageNode)[9].GetAttributeValue("content", "").Split('?')[0];

            HtmlNodeCollection n = doc.DocumentNode.SelectNodes("/html/body/div[3]/main/div[2]/div[2]/div/div[1]");

            ingredients.Add(recipeName);
            ingredients.Add("ImgSrc" + SEPERATOR + img);

            index = 1;
            int colIndex = 1;
            bool isIngredients = true;

            while (isIngredients)
            {
                HtmlNodeCollection textNode = doc.DocumentNode.SelectNodes(String.Format(ingredientTextNode, colIndex, index));

                if (textNode != null)
                {
                    for (int i = 0; i < textNode.Count; i++)
                    {
                        string ing = textNode[i].InnerText.Trim(TRIM_CHARS).Replace('\n', ' ').Replace("&frac12;", "1/2").Replace("&frac14;", "1/4").Replace("&frac34;", "3/4").Replace("&frac13;", "1/3");
                        if (!ing.Equals(""))
                        {
                            if (IsDigitOrSlashOnly(ing.Substring(0, ing.IndexOf(' '))))
                            {
                                ingredients.Add(ing.Substring(0, ing.IndexOf(' ')) + SEPERATOR + ing.Substring(ing.IndexOf(' ') + 1).Trim(TRIM_CHARS));
                            }
                            else
                            {
                                ingredients.Add(ing);
                            }
                        }
                    }
                    index++;
                }
                else
                {
                    if (index == 1)
                    {
                        isIngredients = false;
                    }
                    else
                    {
                        colIndex++;
                        index = 1;
                    }
                }

            }

            ingredients.Add(STEP_SEPERATOR);
            bool isSteps = true;
            index = 1;

            while (isSteps)
            {
                HtmlNodeCollection steps = doc.DocumentNode.SelectNodes(String.Format(stepsNode, index));

                if (steps != null)
                {
                    string step = steps[0].InnerText.Trim(TRIM_CHARS);
                    if (!step.Equals(""))
                    {
                        ingredients.Add(step);
                    }
                    index++;
                }
                else
                {
                    isSteps = false;
                }

            }

            return ingredients.ToArray();
        }


        private static bool IsDigitOrSlashOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    if (c != '/')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }

    class GZipWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request;
        }
    }

    class CookieAwareWebClient : WebClient
    {
        public string Method;
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }

        public CookieAwareWebClient()
            : this(new CookieContainer())
        {
        }

        public CookieAwareWebClient(CookieContainer cookies)
        {
            this.CookieContainer = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.CookieContainer;
                (request as HttpWebRequest).ServicePoint.Expect100Continue = false;
                (request as HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
                (request as HttpWebRequest).Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                (request as HttpWebRequest).Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
                (request as HttpWebRequest).Referer = "http://portal.movable.com/signin";
                (request as HttpWebRequest).KeepAlive = true;
                (request as HttpWebRequest).AutomaticDecompression = DecompressionMethods.Deflate |
                                                                     DecompressionMethods.GZip;
                if (Method == "POST")
                {
                    (request as HttpWebRequest).ContentType = "application/x-www-form-urlencoded";
                }

            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            if (setCookieHeader != null)
            {
                //do something if needed to parse out the cookie.
                try
                {
                    if (setCookieHeader != null)
                    {
                        Cookie cookie = new Cookie(); //create cookie
                        this.CookieContainer.Add(cookie);
                    }
                }
                catch (Exception)
                {

                }
            }
            return response;

        }
    }
}
