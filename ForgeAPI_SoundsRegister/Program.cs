using System;
using System.IO;
using System.Collections.Generic;


namespace ForgeAPI_SoundsRegister
{
    class Program
    {
        public static ushort GetMaxLengthInStringArray(string[] _Strs)
        {
            ushort _MaxLength = 0;
            foreach (string _Str in _Strs)
            {
                if (_Strs.Length > _MaxLength) _MaxLength = (ushort)_Strs.Length;
            }
            return _MaxLength;
        }
        public static char[] CharArrayFrom(string _str)
        {
            return (_str.ToCharArray());
        }
        public static string StringFrom(char[] _CharArray)

        {
            return (new string(_CharArray));
        }
        public static string GetJavaSrcPath(string _MainDirectory, string _package)
        {
            return (_MainDirectory + "\\java\\" + ((_package).Replace(".","\\")));
        }
        public static string GetAllBeforeChrIn(string _Str, char _Chr)
        {
            int _Iterrator = _Str.Length;
            while (_Str[--_Iterrator] != _Chr) {; }
            char[] _ResultAsCharArray = new char[_Iterrator];
            Array.Copy(CharArrayFrom(_Str), 0, _ResultAsCharArray, 0, _Iterrator);
            return StringFrom(_ResultAsCharArray);
        }
        public static string GetAllAfterChrIn(string _Str, char _Chr)
        {

            int _Iterrator = _Str.Length;
            while (_Str[--_Iterrator] != '.') { };
            char[] _StrWithoutChrAsCharArray = new char[_Str.Length - _Iterrator - 1];
            Array.Copy(CharArrayFrom(_Str), _Iterrator + 1, _StrWithoutChrAsCharArray, 0, (_Str.Length - _Iterrator - 1));
            return StringFrom(_StrWithoutChrAsCharArray);
        }
        public static string ModidFrom(string _package)
        {
            int _Iterrator = _package.Length;
            while (_package[--_Iterrator] != '.') { };
            char[] _Modid = new char[_package.Length - _Iterrator - 1];
            Array.Copy(CharArrayFrom(_package), _Iterrator + 1, _Modid, 0, (_package.Length - _Iterrator - 1));
            return StringFrom(_Modid);
        }
        public static string FullFileNameFrom(string _FullFilePath, string _Directory)
        {
            char[] _ResultAsCharArray = new char[(_FullFilePath.Length - _Directory.Length)];
            Array.Copy(CharArrayFrom(_FullFilePath), _Directory.Length + 1, _ResultAsCharArray, 0, _FullFilePath.Length - 1 - _Directory.Length);
            return StringFrom(_ResultAsCharArray);
        }
        public static string NameWithoutExtensionFrom(string _FullFileName)
        {
            int _Iterrator = _FullFileName.Length;
            while (_FullFileName[--_Iterrator] != '.') {; }
            char[] _ResultAsCharArray = new char[_Iterrator];
            Array.Copy(CharArrayFrom(_FullFileName), 0, _ResultAsCharArray, 0, _Iterrator);
            return StringFrom(_ResultAsCharArray);
        }
        public static string SoundNameWitoutPostfixFrom(string _SoundName)
        {
            int _Iterrator = _SoundName.Length;
            while (_SoundName[--_Iterrator] != '_') {; }
            char[] _ResultAsCharArray = new char[_Iterrator];
            Array.Copy(CharArrayFrom(_SoundName), 0, _ResultAsCharArray, 0, _Iterrator);
            return StringFrom(_ResultAsCharArray);

        }
        public static string SoundFilePostfixFrom(string _FileName)
        {

            int _Iterrator = _FileName.Length;
            while (_FileName[--_Iterrator] != '_') {; }
            char[] _ResultAsCharArray = new char[_FileName.Length - 1 - _Iterrator];
            Array.Copy(CharArrayFrom(_FileName), (_Iterrator + 1), _ResultAsCharArray, 0, (_FileName.Length - 1 - _Iterrator));
            return StringFrom(_ResultAsCharArray);
        }
        public static string SoundSoundsDotJsonTemplateSubstitution(string _FileName, string _Modid)
        {
            return ("    \""+_FileName+"\": {\n        \"category\": \"player\",\n        \"sounds\":[ \""+_Modid+":"+_FileName+"\" ]\n\t},\n\t\n");
        }
        public static string MusicSoundsDotJsonTemplateSubstitution(string _FileName, string _Modid)
        {
            return ("  \""+_FileName+"\": {\n    \"category\": \"record\",\n    \"sounds\": [\n      {\n        \"name\": \""+_Modid+":music/"+_FileName+"\",\n        \"stream\": true\n      }\n    ]\n  },\n");
        }
        public static string SoundLocationTemplateSubstitution(string _FileName, string _Modid, ushort _MaxLength)
        {
            return "\tpublic static ResourceLocation " + SoundNameWitoutPostfixFrom(_FileName) + "_location" + (new string(' ', (_MaxLength - _FileName.Length)))+" = new ResourceLocation(\"" + _Modid + "\", \"" + _FileName + "\");\n";
        }
        public static string SoundEventTemplateSubstitution(string _FileName, ushort _MaxLength)
        {
            return ("\tpublic static SoundEvent " + SoundNameWitoutPostfixFrom(_FileName) + (new string(' ', (_MaxLength - _FileName.Length)))+ " = new SoundEvent(SoundLocations." + SoundNameWitoutPostfixFrom(_FileName) + "_location);\n");
        }
        private static void CreateSoundsDotJsonFile(string _Dir, string[] _SoundFilesNames, string _Modid)
        {
            StreamWriter _SoundsDotJsonFile = new StreamWriter((_Dir + "\\sounds.json"));
            _SoundsDotJsonFile.Write("{\n");
            foreach(string _SoundFileName in _SoundFilesNames)
            {
                if (((SoundFilePostfixFrom(_SoundFileName)).Equals("sound")))
                {
                    _SoundsDotJsonFile.Write(SoundSoundsDotJsonTemplateSubstitution(_SoundFileName, _Modid));
                }
                else _SoundsDotJsonFile.Write(MusicSoundsDotJsonTemplateSubstitution(_SoundFileName, _Modid));
            }
            _SoundsDotJsonFile.Close();
        }
        private static void CreateSoundLocationFile(string _Dir, string[] _SoundFilesNames, string _Modid, string _package)
        {
            StreamWriter _SoundLocationFile = new StreamWriter(_Dir + "\\SoundLocation.java");
            _SoundLocationFile.Write("package "+_package+".client.sounds;\n\nimport net.minecraft.util.ResourceLocation;\nimport net.minecraft.util.SoundEvent;\n\npublic class SoundLocations {\n	\n");
            ushort _MaxLength = GetMaxLengthInStringArray(_SoundFilesNames);
            foreach (string _SoundFile in _SoundFilesNames)
            {
                _SoundLocationFile.Write((SoundLocationTemplateSubstitution(_SoundFile, _Modid, _MaxLength)));
            }
            _SoundLocationFile.Write("\t\n}");
            _SoundLocationFile.Close();
        }
        private static void CreateSoundEventFile(string _Dir, string[] _SoundFilesNames, string _Modid, string _package)
        {
            StreamWriter _SoundEventFile = new StreamWriter(_Dir + "\\SoundEvents.java");
            _SoundEventFile.Write("package " + _package + ".client.sounds;\n\nimport net.minecraft.util.ResourceLocation;\nimport net.minecraft.util.SoundEvent;\n\npublic class SoundEvents {\n\n");
            ushort _MaxLength = GetMaxLengthInStringArray(_SoundFilesNames);
            foreach (string _File in _SoundFilesNames)
            {
                _SoundEventFile.Write(SoundEventTemplateSubstitution(_File, _MaxLength));
            }
            _SoundEventFile.Write("\t\n}");
            _SoundEventFile.Close();
        }
        private static void CreateDirectoryIfItDoesntExist(string _DirPath)
        {
            if (!Directory.Exists(_DirPath))
            {
                CreateDirectoryIfItDoesntExist(GetAllBeforeChrIn(_DirPath, '\\'));
                Directory.CreateDirectory(_DirPath);

            }
        }
        static void Main(string[] args)
        {
            string Dir;
            string Package;
        Start:
            if (args.Length > 1)
            {
                Dir = args[0];
                Package = args[1];
            }
            else
            {
                Console.WriteLine("Enter the directory of project");
                Dir = Console.ReadLine();
                Console.WriteLine("Enter the package");
                Package = Console.ReadLine();
            }
            if (!(Directory.Exists(Dir = Dir + "\\src\\main")))
            {
                Console.WriteLine("Entered directory doesn\'t exists, please enter a valid directory");
                if (args.Length > 0) goto End; else goto Start;
            }
            string Modid = ModidFrom(Package);
            string[] Files;
            if (Directory.Exists((Dir + "\\resources\\assets\\"+Modid+"\\sounds")))
            {
                Files = Directory.GetFiles((Dir + "\\resources\\assets\\"+Modid+"\\sounds"));
            }
            else
            {
                Console.WriteLine("Entered project directory doesn\'t contains the sounds assets directory");
                if (args.Length > 0) goto End; else goto Start;
            }
            List<string> FileList = new List<string>();
            foreach (string File in Files)
            {
                if ((File[File.Length - 1] == 'g') &&
                    (File[File.Length - 2] == 'g') &&
                    (File[File.Length - 3] == 'o') &&
                    (File[File.Length - 4] == '.'))
                {
                    FileList.Add(NameWithoutExtensionFrom(FullFileNameFrom(File, (Dir + "\\resources\\assets\\"+Modid+"\\sounds"))));
                }
            }
            string[] FileNames = FileList.ToArray();
            Console.WriteLine("First stage done (1/4)");
            CreateSoundsDotJsonFile((Dir + "\\resources\\assets\\" + Modid), FileNames, Modid);
            Console.WriteLine("Json sucsessfully created (2/4)");
            CreateSoundLocationFile(((GetJavaSrcPath(Dir, Package))+"\\client\\sounds"), FileNames, Modid, Package);
            Console.WriteLine("SoundLocation.java sucsessfully created (3/4)");
            CreateSoundEventFile(((GetJavaSrcPath(Dir, Package)) + "\\client\\sounds"), FileNames, Modid, Package);
            Console.WriteLine("SoundEvents.java sucsessfully created (4/4)");
            Console.WriteLine("Done!");
            if (args.Length < 1) Console.ReadKey();
            End:;
        }
    }
}