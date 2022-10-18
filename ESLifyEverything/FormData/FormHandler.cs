﻿using Mutagen.Bethesda.Plugins;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.FormData;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;

namespace ESLifyEverything.FormData
{
    //Handles anything that specificly needs to be done with the FormKey
    public class FormHandler : IFormHandler
    {
        public FormHandler(){}

        //Creates the Form Data from a xEditLogLine
        public FormHandler(string xEditLogCompactedLine)
        {
            CreateCompactedForm(xEditLogCompactedLine);
        }
        
        //Creates the Form Data from a xEditLogLine
        public void CreateCompactedForm(string xEditLogCompactedLine)
        {
            string logLineFilter = GF.stringsResources.xEditCompactedFormFilter;
            OriginalFormID = xEditLogCompactedLine.Substring(xEditLogCompactedLine.IndexOf(logLineFilter) + logLineFilter.Length + 2, 6);
            
            xEditLogCompactedLine = xEditLogCompactedLine.Substring(xEditLogCompactedLine.IndexOf('"') + 1);
            ModName = xEditLogCompactedLine.Substring(0, xEditLogCompactedLine.IndexOf('"'));

            CompactedFormID = xEditLogCompactedLine.Substring(xEditLogCompactedLine.LastIndexOf('[') + 3, 6);

            if (OriginalFormID.Equals(CompactedFormID))
            {
                IsModified = false;
            }
        }

        //Gets the Origonal FormID without extra 0's infront of formID
        public string GetOriginalFormID()
        {
            return OriginalFormID.TrimStart('0');
        }

        //Gets the Compacted FormID with the same length as the Origonal FormID
        public string GetCompactedFormID()
        {
            int len = 6 - GetOriginalFormID().Length;
            if (len > 3)
            {
                len = 3;
            }
            return CompactedFormID.Substring(len);
        }

        //Gets the Compacted FormID without extra 0's infront of the FormID
        public string GetCompactedFormIDTrimmed()
        {
            return CompactedFormID.TrimStart('0');
        }

        //Creates the Origonal FormKey with separator data passed in
        public string GetOriginalFileLineFormKey(Separator separator, string orgModName)
        {
            string modName = orgModName;
            if (separator.ModNameAsString)
            {
                modName = separator.ModNameStringCharater + orgModName + separator.ModNameStringCharater;
            }

            if (separator.IDIsSecond)
            {
                return modName + separator.FormKeySeparator + GetOriginalFormID();
            }
            return GetOriginalFormID() + separator.FormKeySeparator + modName;
        }

        //Creates the Compacted FormKey with separator data passed in
        public string GetCompactedFileLineFormKey(Separator separator)
        {
            string modName = ModName;
            if (separator.ModNameAsString)
            {
                modName = separator.ModNameStringCharater + ModName + separator.ModNameStringCharater;
            }

            if (separator.IDIsSecond)
            {
                return modName + separator.FormKeySeparator + GetCompactedFormIDTrimmed();
            }
            return GetCompactedFormIDTrimmed() + separator.FormKeySeparator + modName;
        }

        //Creates a string of the Object's data for logging usually
        public new string ToString()
        {
            return "Mod Name: " + ModName + " | Origonal FormID: " + OriginalFormID + " | Compacted FormID: " + CompactedFormID + " | IsModified: " + IsModified;
        }

        //Creates the Mutagen FormKey related to the Origonal Form
        public FormKey CreateOriginalFormKey(string orgModName)
        {
            FormKey.TryFactory($"{OriginalFormID}:{orgModName}", out FormKey origonalFormKey);
            return origonalFormKey;
        }

        //Creates the Mutagen FormKey related to the Compacted Form
        public FormKey CreateCompactedFormKey()
        {
            FormKey.TryFactory($"{CompactedFormID}:{ModName}", out FormKey compactedFormKey);
            return compactedFormKey;
        }

    }
}
