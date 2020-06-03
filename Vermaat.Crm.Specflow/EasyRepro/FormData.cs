using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro.Fields;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormData
    {
        private readonly UCIApp _app;
        private readonly EntityMetadata _entityMetadata;
        private readonly Dictionary<string, FormField> _formFields;

        public FormField this[string attributeName] => _formFields[attributeName];
        public CommandBarActions CommandBar { get; }

        public FormData(UCIApp app, EntityMetadata entityMetadata)
        {
            _app = app;
            _entityMetadata = entityMetadata;
            CommandBar = new CommandBarActions(_app);

            _formFields = InitializeFormData();
        }

        public void ClickSubgridButton(string subgridName, string subgridButton)
        {
            _app.ExecuteSeleniumFunction((driver, selectors) =>
            {
                var subGrid = driver.WaitUntilAvailable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid, subgridName), $"Unable to find subgrid: {subgridName}");
                var menuBar = subGrid.FindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid_ButtonList));
                var buttons = menuBar.FindElements(By.TagName("button"));
                var button = buttons.FirstOrDefault(b => b.GetAttribute("data-id").Contains(subgridButton));
                if (button != null)
                {
                    button.Click();
                    return true;
                }

                var moreCommands = buttons.FirstOrDefault(b => b.GetAttribute("data-id").Equals("OverflowButton"));
                if (moreCommands == null)
                    throw new TestExecutionException(Constants.ErrorCodes.MORE_COMMANDS_NOT_FOUND);
                moreCommands.Click();

                var flyout = driver.FindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.FlyoutRoot));
                flyout.FindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid_Button, subgridButton)).Click();

                return true;
            });
        }

        public bool ContainsField(string fieldLogicalName)
        {
            var containsField = _formFields.ContainsKey(fieldLogicalName);
            Logger.WriteLine($"Field {fieldLogicalName} is on Form: {containsField}");
            return containsField;
        }

        public string GetErrorDialogMessage()
        {
            Logger.WriteLine("Getting error dialog message");
            return _app.ExecuteSeleniumFunction((driver, selectors) =>
            {
                return SeleniumFunctions.GetErrorDialogMessage(driver, selectors);
            });
            
        }

        public IReadOnlyCollection<FormNotification> GetFormNotifications()
        {
            return _app.ExecuteSeleniumFunction((driver, selectors) =>
            {
                List<FormNotification> notifications = new List<FormNotification>();

                if (!driver.TryFindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationBar),
                    out var notificationBar))
                {
                    return notifications;
                }

                if (notificationBar.TryFindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_ExpandButton), out var expandButton))
                {
                    if (!Convert.ToBoolean(notificationBar.GetAttribute("aria-expanded")))
                        expandButton.Click();

                    notificationBar = driver.WaitUntilAvailable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.FlyoutRoot), TimeSpan.FromSeconds(2), "Failed to open the form notifications");
                }

                var notificationList = notificationBar.FindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationList));
                var notificationListItems = notificationList.FindElements(By.TagName("li"));

                foreach (var item in notificationListItems)
                {
                    var icon = item.FindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationTypeIcon));

                    var notification = new FormNotification
                    {
                        Message = item.GetAttribute("aria-label")
                    };

                    if (icon.HasClass("MarkAsLost-symbol"))
                        notification.Type = FormNotificationType.Error;
                    else if (icon.HasClass("Warning-symbol"))
                        notification.Type = FormNotificationType.Warning;
                    else if (icon.HasClass("InformationIcon-symbol"))
                        notification.Type = FormNotificationType.Information;
                    else
                        throw new TestExecutionException(Constants.ErrorCodes.UNKNOWN_FORM_NOTIFICATION_TYPE, icon.GetAttribute("class"));

                    notifications.Add(notification);
                }
                return notifications;
            });
        }

        public Guid GetRecordId()
        {
            Logger.WriteLine("Getting Record Id");
            var id = _app.App.Entity.GetObjectId();
            Logger.WriteLine($"Record ID of current opened record: {id}");
            return id;
        }

        public void Save(bool saveIfDuplicate)
        {
            Logger.WriteLine($"Saving Record");
            try
            {
                _app.App.Entity.Save();
            }
            catch(InvalidOperationException ex)
            {
                throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, ex, ex.Message);
            }
            ConfirmDuplicate(saveIfDuplicate);
            WaitUntilSaveCompleted();
        }

        public void FillForm(CrmTestingContext crmContext, Table formData)
        {
            Logger.WriteLine($"Filling form");
            var formState = new FormState(_app);
            foreach (var row in formData.Rows)
            {
                Assert.IsTrue(ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                var field = _formFields[row[Constants.SpecFlow.TABLE_KEY]];
                Assert.IsTrue(field.IsVisible(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't visible");
                Assert.IsFalse(field.IsLocked(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} is read-only");

                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }

        private void WaitUntilSaveCompleted()
        {
            var timeout = DateTime.Now.AddSeconds(20);
            bool saveCompleted = false;
            while (!saveCompleted && DateTime.Now < timeout)
            {
                var footerElement = _app.WebDriver.FindElement(By.XPath("//span[@data-id='edit-form-footer-message']"));

                if (!string.IsNullOrEmpty(footerElement.Text) && footerElement.Text.ToLower() == "saving")
                {
                    Logger.WriteLine("Save not yet completed. Waiting..");
                    Thread.Sleep(500);
                }
                else if(!string.IsNullOrEmpty(footerElement.Text) && footerElement.Text.ToLower() == "unsaved changes")
                {
                    var formNotifications = GetFormNotifications();
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, $"Detected Unsaved changes. Form Notifications: {string.Join(", ", formNotifications)}");
                }
                else
                {
                    Logger.WriteLine("Save sucessfull");
                    saveCompleted = true;
                }
            }

            if (!saveCompleted)
                throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_TIMEOUT, 20);
        }

        private void ConfirmDuplicate(bool saveIfDuplicate)
        {
            var element = _app.WebDriver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]), new TimeSpan(0, 0, 5));

            if(element != null)
            {
                if (saveIfDuplicate)
                {
                    _app.WebDriver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]));
                }
                else
                {
                    throw new TestExecutionException(Constants.ErrorCodes.DUPLICATE_RECORD_DETECTED);
                }
            }
        }

        private Dictionary<string, FormField> InitializeFormData()
        {
            dynamic attributeCollection = _app.WebDriver.ExecuteScript("return Xrm.Page.data.entity.attributes.getAll().map(function(a) { return { name: a.getName(), controls: a.controls.getAll().map(function(c) { return c.getName() }) } })");

            var formFields = new Dictionary<string, FormField>();
            var metadataDic = _entityMetadata.Attributes.ToDictionary(a => a.LogicalName);
            foreach (var attribute in attributeCollection)
            {
                var controls = new string[attribute["controls"].Count];

                for (int i = 0; i < attribute["controls"].Count; i++)
                {
                    controls[i] = attribute["controls"][i];
                }

                FormField field = CreateFormField(metadataDic[attribute["name"]], controls);
                if (field != null)
                    formFields.Add(attribute["name"], field);
                
            }

            return formFields;
        }

        private FormField CreateFormField(AttributeMetadata metadata, string[] controls)
        {
            if (controls.Length == 0)
                return null;

            // Take the first control that isn't in the header
            for(int i = 0; i < controls.Length; i++)
            {
                if(!controls[i].StartsWith("header"))
                {
                    return new BodyFormField(_app, metadata, controls[i]);
                }
            }
            // If all are in the header, take the first header control
            return new HeaderFormField(_app, metadata, controls[0]);
        }
    }
}
