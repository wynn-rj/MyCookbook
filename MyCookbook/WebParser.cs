using HtmlAgilityPack;
using System;
using System.Collections.Generic;

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
            HtmlDocument doc = web.Load(url);
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
            HtmlNodeCollection h = doc.DocumentNode.SelectNodes(imageNode);
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
            HtmlNodeCollection h = doc.DocumentNode.SelectNodes(imageNode);
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
}
