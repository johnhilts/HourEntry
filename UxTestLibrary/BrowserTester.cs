using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using WatiN.Core;

namespace UxTestLibrary
{
    /// <summary>
    /// Wrapper around WatiN functionality
    /// </summary>
    public class BrowserTester
    {
        private IE _ie;

        public BrowserTester()
        {
            //this._ie = new IE();
        }

        public Uri CurrentUrl
        {
            get { return new Uri(this._ie.Url); }
        }

        public void NavigateTo(Uri url)
        {
            this.NavigateTo(url, url);
        }

        public void NavigateTo(Uri testUrl, Uri expectedUrl)
        {
            try
            {
                if (this._ie == null)
                {
                    this._ie = new IE(testUrl);
                }
                else
                {
                    this._ie.GoTo(testUrl.ToString());
                }
            }
            catch (WatiN.Core.Exceptions.TimeoutException tex)
            {
                string errorMessage = "";
                if (this._ie == null)
                    errorMessage = "IE object NULL";
                else
                    this._ie.Close(); // won't this cause trouble in the TearDown?? not if this is after the first navigation
                errorMessage += "\r\nIE Timed out\r\nError Info:\r\n" + tex.ToString();
                Assert.Fail(errorMessage);
            }

            //string ieSource = this._ie.Html;
            //Console.WriteLine("******************************************");
            //Console.WriteLine("HTML Source for " + testUrl.ToString());
            //Console.WriteLine("******************************************");
            //Console.WriteLine(ieSource);
            //Console.WriteLine("******************************************\r\n");

            Assert.That(this._ie.Url.ToLower(), Is.EqualTo(expectedUrl.OriginalString.ToLower()), "Wrong URL");
        }

        public void InputText(string id, string controlName, string inputText)
        {
            TextField txt = this._ie.TextField(Find.ById(new Regex(id)));
            Assert.That(txt.Exists, Is.True, "No {0} Text Field", controlName);
            txt.TypeText(inputText);
        }

        public void CheckCheckBox(string id, string controlName)
        {
            CheckBox cb = this._ie.CheckBox(Find.ById(new Regex(id)));
            Assert.That(cb.Exists, Is.True, "No {0} Check Box", controlName);
            cb.Click();
        }

        public void CheckRadioButton(string id, string controlName)
        {
            this.CheckRadioButton(id, controlName, this._ie);
        }

        public void CheckRadioButton(string id, string controlName, IE ie)
        {
            RadioButton rb = ie.RadioButton(Find.ById(new Regex(id)));
            Assert.That(rb.Exists, Is.True, "No {0} Radio Button", controlName);
            rb.Click();
        }

        public void ChooseDropDownOption(string id, string controlName, string optionValueChoice)
        {
            SelectList sel = this._ie.SelectList(Find.ById(new Regex(id)));
            Assert.That(sel.Exists, Is.True, "No {0} Dropdown List", controlName);
            sel.SelectByValue(optionValueChoice);
        }

        public void ClickButton(string id, string controlName)
        {
            ClickButton(id, controlName, this._ie);
        }

        public void ClickButton(string id, string controlName, IE ie)
        {
            ClickButton(id, controlName, this._ie, false);
        }

        public void ClickButton(string id, string controlName, IE ie, bool noWait)
        {
            Button btn = ie.Button(Find.ById(new Regex(id)));
            Assert.That(btn.Exists, Is.True, "No {0} Button", controlName);
            if (noWait)
                btn.ClickNoWait(); // this is good with buttons in popups that result in closing the popup
            else
                btn.Click();
        }

        public void SubmitForm()
        {
            Form form = this._ie.Forms[0];
            form.Submit();
        }

        /// <summary>
        /// Find a link with supplied path and navigates to the path by clicking the link
        /// </summary>
        /// <param name="path">supply a URL or path</param>
        /// <param name="name">name of the link - mostly for convenience</param>
        /// <param name="verifyUrlAfterNavigation">If false, don't verify that the URL after clicking the link matches what was in the link.  This is useful when the link redirects so that client can 
        /// perform a different check. If true then this method will verify that the URL navigated to matches the URL in the clicked link.</param>
        public void NavigateByLinks(string path, string name, bool verifyUrlAfterNavigation)
        {
            Link link = this._ie.Link(Find.ByUrl(new Regex(path, RegexOptions.IgnoreCase)));
            Assert.That(link.Exists, Is.True, "Could not find link to: " + name);
            link.Click();
            if (verifyUrlAfterNavigation)
            {
                Assert.That(this._ie.Url.Contains(path), Is.True, "Wrong Page: " + name);
            }
        }

        public void NavigateByLinks(string path, string name)
        {
            this.NavigateByLinks(path, name, true);
        }

        public void Wait()
        {
            this.Wait(Settings.Instance.WaitForCompleteTimeOut);
        }

        public void Wait(int timeoutSeconds)
        {
            Settings.Instance.WaitForCompleteTimeOut = timeoutSeconds;
            Settings.Instance.WaitUntilExistsTimeOut = timeoutSeconds;
            this._ie.WaitForComplete(new WaitForComplete(this._ie.DomContainer));
        }

        /// <summary>
        /// Verify that text field's text is equal to supplied string
        /// </summary>
        /// <param name="id">control ID</param>
        /// <param name="controlName">friendly name for convenience</param>
        /// <param name="verifyText">text to verify against</param>
        public void VerifyText(string id, string controlName, string verifyText)
        {
            TextField txt = this._ie.TextField(Find.ById(new Regex(id)));
            Assert.That(txt.Exists, Is.True, "No {0} Text Field", controlName);
            Assert.That(txt.Text, Is.EqualTo(verifyText), "Wrong Text in {0}", controlName);
        }

        /// <summary>
        /// Verify that a text contains at least some text, or contains no text
        /// </summary>
        /// <param name="id">control ID</param>
        /// <param name="controlName">friendly name for convenience</param>
        /// <param name="hasSomeText">When true, verify that control has at least some text; when false verify control has no text</param>
        public void VerifyText(string id, string controlName, bool hasSomeText)
        {
            TextField txt = this._ie.TextField(Find.ById(new Regex(id)));
            Assert.That(txt.Exists, Is.True, "No {0} Text Field", controlName);
            if (hasSomeText)
            {
                Assert.That(txt.Text, Is.Not.Null & Is.Not.Empty, "Expected to have some text in {0}", controlName);
            }
            else
                Assert.That(txt.Text, Is.Empty, "Expected to have NO text in {0}", controlName);
        }

        // TODO: we can DRY VerifyText and VerifyLabelText with Generics
        public void VerifySpanText(string id, string controlName, string verifyText)
        {
            Span span = this._ie.Span(Find.ById(new Regex(id)));
            Assert.That(span.Exists, Is.True, "No {0} Span", controlName);
            Assert.That(span.Text, Is.EqualTo(verifyText), "Wrong Text in {0}", controlName);
        }

        public void VerifySpanText(string id, string controlName, bool hasSomeText)
        {
            Span span = this._ie.Span(Find.ById(new Regex(id)));
            Assert.That(span.Exists, Is.True, "No {0} Text Field", controlName);
            if (hasSomeText)
                Assert.That(span.Text, Is.Not.Empty, "Expected to have some text in {0}", controlName);
            else
                Assert.That(span.Text, Is.Empty, "Expected to have NO text in {0}", controlName);
        }

        public void VerifyBodyText(string verifyText)
        {
            Assert.That(this._ie.Text.Contains(verifyText), Is.True, "Text not found in body of page");
        }

        // TODO: make this IDisposable?
        public void Close()
        {
            if (this._ie != null)
            {
                this._ie.Close();
            }
        }
    }
}
