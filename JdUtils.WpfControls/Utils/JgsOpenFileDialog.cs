using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JdUtils.WpfControls.Utils
{
    public class JgsOpenFileDialog
    {
        private WindowsFormsAssemblyAccessor m_assemblyAccessor;

        public string InitialDirectory { get; set; }

        public string Title { get; set; }

        public bool Multiselect { get; set; }

        public bool PickFolders { get; set; }

        public string Filter { get; set; }

        public bool AddExtension { get; set; }

        public bool DereferenceLinks { get; set; }

        public string FileName { get; private set; }

        public IList<string> FileNames { get; private set; }

        public bool ShowDialog()
        {
            return ShowDialog(IntPtr.Zero);
        }

        public bool ShowDialog(IntPtr hWndOwner)
        {

            if (Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("Not supported on this OS Version");
            }

            var dialog =  CreateDialog();
            var accessor = m_assemblyAccessor ?? (m_assemblyAccessor = new WindowsFormsAssemblyAccessor());
            var result = accessor.ShowDialog(dialog, hWndOwner);
            if (result)
            {
                FileName = dialog.FileName;
                FileNames = dialog.FileNames?.ToList();
            }
            return result;
        }

        private OpenFileDialog CreateDialog()
        {
            return new OpenFileDialog
            {
                CheckFileExists = false,
                Title = GetTitle(),
                InitialDirectory = GetInitialDirectory(),
                Filter = GetFilter(),
                AddExtension = GetAddExtensions(),
                DereferenceLinks = GetDeferenceLinks(),
                Multiselect = Multiselect
            };
        }

        private string GetTitle()
        {
            return string.IsNullOrEmpty(Title)
                ? CreateDefaultTitle()
                : Title;
        }

        private string CreateDefaultTitle()
        {
            return PickFolders ? "Select a folder" : "Select a file";
        }

        private string GetInitialDirectory()
        {
            return string.IsNullOrEmpty(InitialDirectory)
                ? Environment.CurrentDirectory
                : InitialDirectory;
        }

        private string GetFilter()
        {
            return PickFolders ? "Folder|\n" : Filter;
        }

        private bool GetAddExtensions()
        {
            return PickFolders ? false : AddExtension;
        }

        private bool GetDeferenceLinks()
        {
            return PickFolders ? true : DereferenceLinks;
        }

        //private bool ShowNewDialog(IntPtr hWndOwner)
        //{
        //    var flag = false;
        //    var r = new Reflector("System.Windows.Forms");
        //    uint num = 0;
        //    Type typeIFileDialog = r.GetType("FileDialogNative+IFileDialog");
        //    object dialog = r.Call(m_openFileDialog, "CreateVistaDialog");
        //    uint options = (uint)r.CallAs(typeof(FileDialog), m_openFileDialog, "GetOptions");
        //    options |= (uint)r.GetEnum("FileDialogNative+FOS", "FOS_PICKFOLDERS");
        //    r.CallAs(typeIFileDialog, dialog, "SetOptions", options);
        //    object pfde = r.New("FileDialog.VistaDialogEvents", m_openFileDialog);
        //    object[] parameters = new object[] { pfde, num };
        //    r.CallAs2(typeIFileDialog, dialog, "Advise", parameters);
        //    num = (uint)parameters[1];
        //    try
        //    {
        //        int num2 = (int)r.CallAs(typeIFileDialog, dialog, "Show", hWndOwner);
        //        flag = 0 == num2;
        //    }
        //    finally
        //    {
        //        r.CallAs(typeIFileDialog, dialog, "Unadvise", num);
        //        GC.KeepAlive(pfde);
        //    }

        //    return flag;
        //}

        //public class Reflector
        //{
        //    string m_ns;
        //    Assembly m_asmb;
        //    public Reflector(string ns)
        //        : this(ns, ns)
        //    { }
        //    public Reflector(string an, string ns)
        //    {
        //        m_ns = ns;
        //        m_asmb = null;
        //        foreach (AssemblyName aN in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
        //        {
        //            if (aN.FullName.StartsWith(an))
        //            {
        //                m_asmb = Assembly.Load(aN);
        //                break;
        //            }
        //        }
        //    }
        //    public Type GetType(string typeName)
        //    {
        //        Type type = null;
        //        string[] names = typeName.Split('.');

        //        if (names.Length > 0)
        //            type = m_asmb.GetType(m_ns + "." + names[0]);

        //        for (int i = 1; i < names.Length; ++i)
        //        {
        //            type = type.GetNestedType(names[i], BindingFlags.NonPublic);
        //        }
        //        return type;
        //    }
        //    public object New(string name, params object[] parameters)
        //    {
        //        Type type = GetType(name);
        //        ConstructorInfo[] ctorInfos = type.GetConstructors();
        //        foreach (ConstructorInfo ci in ctorInfos)
        //        {
        //            try
        //            {
        //                return ci.Invoke(parameters);
        //            }
        //            catch { }
        //        }

        //        return null;
        //    }
        //    public object Call(object obj, string func, params object[] parameters)
        //    {
        //        return Call2(obj, func, parameters);
        //    }
        //    public object Call2(object obj, string func, object[] parameters)
        //    {
        //        return CallAs2(obj.GetType(), obj, func, parameters);
        //    }
        //    public object CallAs(Type type, object obj, string func, params object[] parameters)
        //    {
        //        return CallAs2(type, obj, func, parameters);
        //    }
        //    public object CallAs2(Type type, object obj, string func, object[] parameters)
        //    {
        //        MethodInfo methInfo = type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        //        return methInfo.Invoke(obj, parameters);
        //    }
        //    public object Get(object obj, string prop)
        //    {
        //        return GetAs(obj.GetType(), obj, prop);
        //    }
        //    public object GetAs(Type type, object obj, string prop)
        //    {
        //        PropertyInfo propInfo = type.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        //        return propInfo.GetValue(obj, null);
        //    }
        //    public object GetEnum(string typeName, string name)
        //    {
        //        Type type = GetType(typeName);
        //        FieldInfo fieldInfo = type.GetField(name);
        //        return fieldInfo.GetValue(null);
        //    }
        //}
    }
}
