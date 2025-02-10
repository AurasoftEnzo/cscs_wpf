using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfCSCS.Reporting.ReportBuilderReporting
{
    class ReportBuilderDll
    {
        //const string DLLPath = "D:\\DELPHI\\MyDLLReportNevio10\\Win64\\Debug\\MyDLLReportNevio10.dll";
        const string DLLPath = @"C:\Users\User\Documents\GitHub\cscs_wpf\bin\Debug\ReportBuilderDll\MyDLLReportNevio10.dll";

        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLTest();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLFreeDictionary();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLLoadRTM([In] int strLengthFileName,
                                                [In] IntPtr pStrFileName);

        public static UInt32 DLLLoadRTM_(string FileName)
        {
            UInt32 ret = 0;//ERROR

            string s = MyDictionary.RTMFolderFilename;

            if (s == null)
            {
                MessageBox.Show("String FileName je null.\nC# Metoda: DLLLoadRTM_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(s); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLLoadRTM(s.Length, ptr);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLInitDictionary();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLInitReport([In] int reportTAGToFind,
                                                   [In] int parentTAG,
                                                   [Out] IntPtr pReportHandle);

        public static UInt32 DLLInitReport_(int reportTAGToFind,
                                            int parentTAG,
                                            out int reportHandle)
        {
            UInt32 x = 0;//ERROR            

            reportHandle = 0;

            if (reportTAGToFind == 0)
            {
                MessageBox.Show("reportTAGToFind je nula.\nC# Metoda: DLLInitReport_");
                return 0;
            }

            unsafe
            {
                int size = 4;

                fixed (byte* buffer = new byte[size])
                {

                    IntPtr pReportHandle = (IntPtr)buffer;

                    x = DLLInitReport(reportTAGToFind,
                                      parentTAG,
                                      pReportHandle);

                    reportHandle = Marshal.ReadInt32(pReportHandle);

                }
                return x;
            }
        }


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLInitListKomponente([In] int reportHandle);


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLKomponenteListCount([In] int reportHandle,
                                                            [Out] IntPtr pCountOfKomponente);


        public static UInt32 DLLKomponenteListCount_(int reportHandle,
                                                     out int countOfKomponente)
        {
            unsafe
            {
                int size = 4;
                UInt32 x;
                fixed (byte* buffer = new byte[size])
                {

                    IntPtr pCountOfKomponente = (IntPtr)buffer;

                    x = DLLKomponenteListCount(reportHandle,
                                          pCountOfKomponente);

                    countOfKomponente = Marshal.ReadInt32(pCountOfKomponente);

                }
                return x;

            }
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetKomponenta([In] int reportHandle,
                                                      [In] int komponentaIndex,

                                                      [In] int maxSizeBytesName,
                                                      [Out] IntPtr pStrLengthName,
                                                      [Out] IntPtr pStrName,

                                                      [In] int maxSizeBytesClassName,
                                                      [Out] IntPtr pStrLengthClassName,
                                                      [Out] IntPtr pStrClassName
                                                      );

        public static UInt32 DLLGetKomponenta_(int reportHandle,
        int komponentaIndex,
                                               out Komponenta komponenta)
        {
            unsafe
            {
                UInt32 x;

                fixed (byte* buffer1 = new byte[4])
                {
                    IntPtr pStrLengthName = (IntPtr)buffer1;

                    int maxSizeBytesName = 1000;
                    fixed (byte* buffer2 = new byte[maxSizeBytesName])
                    {
                        IntPtr pStrName = (IntPtr)buffer2;

                        fixed (byte* buffer3 = new byte[4])
                        {
                            IntPtr pStrLengthClassName = (IntPtr)buffer3;

                            int maxSizeBytesClassName = 1000;
                            fixed (byte* buffer4 = new byte[maxSizeBytesClassName])
                            {
                                IntPtr pStrClassName = (IntPtr)buffer4;

                                x = DLLGetKomponenta(reportHandle, komponentaIndex,
                                                     maxSizeBytesName, pStrLengthName, pStrName,
                                                     maxSizeBytesClassName, pStrLengthClassName, pStrClassName
                                                     );

                                string Name;
                                string ClassName;

                                int strLengthName;
                                strLengthName = Marshal.ReadInt32(pStrLengthName);
                                Name = Marshal.PtrToStringUni(pStrName, strLengthName);

                                int strLengthClassName;
                                strLengthClassName = Marshal.ReadInt32(pStrLengthClassName);
                                ClassName = Marshal.PtrToStringUni(pStrClassName, strLengthClassName);

                                komponenta = new Komponenta();
                                komponenta.Name = Name;
                                komponenta.ClassName = ClassName;
                            }
                        }
                    }
                }
                return x;
            }
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetDBTextType([In] int reportHandle,
                                                      [In] int indexVariableDBText,
                                                      [In] int strLengthType,
                                                      [In] IntPtr pStrType,
                                                      [In] int size);

        public static UInt32 DLLSetDBTextType_(int reportHandle,
                                               int indexVariableDBText,
                                               string tip,
                                               int size)
        {
            UInt32 ret = 0;//ERROR            

            if (tip == null)
            {
                MessageBox.Show("String: tip je null.\nC# Metoda: DLLSetDBTextType_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(tip); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetDBTextType(reportHandle, indexVariableDBText, tip.Length, ptr, size);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetDBMemoType([In] int reportHandle,
                                                      [In] int indexVariableDBMemo,
                                                      [In] int strLengthType,
                                                      [In] IntPtr pStrType,
                                                      [In] int size);

        public static UInt32 DLLSetDBMemoType_(int reportHandle,
                                               int indexVariableDBMemo,
                                               string tip,
                                               int size)
        {
            UInt32 ret = 0;//ERROR            

            if (tip == null)
            {
                MessageBox.Show("String: tip je null.\nC# Metoda: DLLSetDBMemoType_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(tip); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetDBMemoType(reportHandle, indexVariableDBMemo, tip.Length, ptr, size);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetDBRichTextType([In] int reportHandle,
                                                          [In] int indexVariableDBRichText,
                                                          [In] int strLengthType,
                                                          [In] IntPtr pStrType,
                                                          [In] int size);

        public static UInt32 DLLSetDBRichTextType_(int reportHandle,
                                                   int indexVariableDBRichText,
                                                   string tip,
                                                   int size)
        {
            UInt32 ret = 0;//ERROR            

            if (tip == null)
            {
                MessageBox.Show("String: tip je null.\nC# Metoda: DLLSetDBRichTextType_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(tip); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetDBRichTextType(reportHandle, indexVariableDBRichText, tip.Length, ptr, size);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetDBImageType([In] int reportHandle,
                                                       [In] int indexVariableDBImage,
                                                       [In] int strLengthType,
                                                       [In] IntPtr pStrType,
                                                       [In] int size);

        public static UInt32 DLLSetDBImageType_(int reportHandle,
                                                int indexVariableDBImage,
                                                string tip,
                                                int size)
        {
            UInt32 ret = 0;//ERROR            

            if (tip == null)
            {
                MessageBox.Show("String: tip je null.\nC# Metoda: DLLSetDBImageType_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(tip); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetDBImageType(reportHandle, indexVariableDBImage, tip.Length, ptr, size);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetDBCheckBoxType([In] int reportHandle,
                                                          [In] int indexVariableDBCheckBox,
                                                          [In] int strLengthType,
                                                          [In] IntPtr pStrType,
                                                          [In] int size);

        public static UInt32 DLLSetDBCheckBoxType_(int reportHandle,
                                                   int indexVariableDBCheckBox,
                                                   string tip,
                                                   int size)
        {
            UInt32 ret = 0;//ERROR            

            if (tip == null)
            {
                MessageBox.Show("String: tip je null.\nC# Metoda: DLLSetDBCheckBoxType_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(tip); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetDBCheckBoxType(reportHandle, indexVariableDBCheckBox, tip.Length, ptr, size);
                }
            }

            return ret;
        }




        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetDBTextDataField([In] int reportHandle,
                                                           [In] int komponentaIndex,

                                                           [In] int maxSizeBytesDataField,
                                                           [Out] IntPtr pStrLengthDataField,
                                                           [Out] IntPtr pStrDataField
                                                      );

        public static UInt32 DLLGetDBTextDataField_(int reportHandle,
                                                    int komponentaIndex,
                                                    out string DataField
                                                    )
        {
            unsafe
            {
                UInt32 x;

                fixed (byte* buffer1 = new byte[4])
                {
                    IntPtr pStrLengthDataField = (IntPtr)buffer1;

                    int maxSizeBytesDataField = 1000;
                    fixed (byte* buffer2 = new byte[maxSizeBytesDataField])
                    {
                        IntPtr pStrDataField = (IntPtr)buffer2;

                        x = DLLGetDBTextDataField(reportHandle, komponentaIndex,
                                                 maxSizeBytesDataField, pStrLengthDataField, pStrDataField
                                                 );

                        int strLengthDataField;
                        strLengthDataField = Marshal.ReadInt32(pStrLengthDataField);
                        DataField = Marshal.PtrToStringUni(pStrDataField, strLengthDataField);
                    }
                }
                return x;
            }
        }

        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetDBMemoDataField([In] int reportHandle,
                                                           [In] int komponentaIndex,

                                                           [In] int maxSizeBytesDataField,
                                                           [Out] IntPtr pStrLengthDataField,
                                                           [Out] IntPtr pStrDataField
                                                      );

        public static UInt32 DLLGetDBMemoDataField_(int reportHandle,
                                                    int komponentaIndex,
                                                    out string DataField
                                                    )
        {
            unsafe
            {
                UInt32 x;

                fixed (byte* buffer1 = new byte[4])
                {
                    IntPtr pStrLengthDataField = (IntPtr)buffer1;

                    int maxSizeBytesDataField = 1000;
                    fixed (byte* buffer2 = new byte[maxSizeBytesDataField])
                    {
                        IntPtr pStrDataField = (IntPtr)buffer2;

                        x = DLLGetDBMemoDataField(reportHandle, komponentaIndex,
                                                 maxSizeBytesDataField, pStrLengthDataField, pStrDataField
                                                 );

                        int strLengthDataField;
                        strLengthDataField = Marshal.ReadInt32(pStrLengthDataField);
                        DataField = Marshal.PtrToStringUni(pStrDataField, strLengthDataField);
                    }
                }
                return x;
            }
        }


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetDBRichTextDataField([In] int reportHandle,
                                                               [In] int komponentaIndex,

                                                               [In] int maxSizeBytesDataField,
                                                               [Out] IntPtr pStrLengthDataField,
                                                               [Out] IntPtr pStrDataField
                                                               );

        public static UInt32 DLLGetDBRichTextDataField_(int reportHandle,
                                                        int komponentaIndex,
                                                        out string DataField
                                                       )
        {
            unsafe
            {
                UInt32 x;

                fixed (byte* buffer1 = new byte[4])
                {
                    IntPtr pStrLengthDataField = (IntPtr)buffer1;

                    int maxSizeBytesDataField = 1000;
                    fixed (byte* buffer2 = new byte[maxSizeBytesDataField])
                    {
                        IntPtr pStrDataField = (IntPtr)buffer2;

                        x = DLLGetDBRichTextDataField(reportHandle, komponentaIndex,
                                                      maxSizeBytesDataField, pStrLengthDataField, pStrDataField
                                                     );

                        int strLengthDataField;
                        strLengthDataField = Marshal.ReadInt32(pStrLengthDataField);
                        DataField = Marshal.PtrToStringUni(pStrDataField, strLengthDataField);
                    }
                }
                return x;
            }
        }


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetDBImageDataField([In] int reportHandle,
                                                            [In] int komponentaIndex,

                                                            [In] int maxSizeBytesDataField,
                                                            [Out] IntPtr pStrLengthDataField,
                                                            [Out] IntPtr pStrDataField
                                                            );

        public static UInt32 DLLGetDBImageDataField_(int reportHandle,
                                                     int komponentaIndex,
                                                     out string DataField
                                                     )
        {
            unsafe
            {
                UInt32 x;

                fixed (byte* buffer1 = new byte[4])
                {
                    IntPtr pStrLengthDataField = (IntPtr)buffer1;

                    int maxSizeBytesDataField = 1000;
                    fixed (byte* buffer2 = new byte[maxSizeBytesDataField])
                    {
                        IntPtr pStrDataField = (IntPtr)buffer2;

                        x = DLLGetDBImageDataField(reportHandle, komponentaIndex,
                                                   maxSizeBytesDataField, pStrLengthDataField, pStrDataField
                                                  );

                        int strLengthDataField;
                        strLengthDataField = Marshal.ReadInt32(pStrLengthDataField);
                        DataField = Marshal.PtrToStringUni(pStrDataField, strLengthDataField);
                    }
                }
                return x;
            }
        }


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetDBCheckBoxDataField([In] int reportHandle,
                                                               [In] int komponentaIndex,

                                                               [In] int maxSizeBytesDataField,
                                                               [Out] IntPtr pStrLengthDataField,
                                                               [Out] IntPtr pStrDataField
                                                               );

        public static UInt32 DLLGetDBCheckBoxDataField_(int reportHandle,
                                                        int komponentaIndex,
                                                        out string DataField
                                                       )
        {
            unsafe
            {
                UInt32 x;

                fixed (byte* buffer1 = new byte[4])
                {
                    IntPtr pStrLengthDataField = (IntPtr)buffer1;

                    int maxSizeBytesDataField = 1000;
                    fixed (byte* buffer2 = new byte[maxSizeBytesDataField])
                    {
                        IntPtr pStrDataField = (IntPtr)buffer2;

                        x = DLLGetDBCheckBoxDataField(reportHandle, komponentaIndex,
                                                   maxSizeBytesDataField, pStrLengthDataField, pStrDataField
                                                  );

                        int strLengthDataField;
                        strLengthDataField = Marshal.ReadInt32(pStrLengthDataField);
                        DataField = Marshal.PtrToStringUni(pStrDataField, strLengthDataField);
                    }
                }
                return x;
            }
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetLabelCaption([In] int reportHandle,
                                                        [In] int komponentaIndex,
                                                        [In] int strLengthCaption,
                                                        [In] IntPtr pStrCaption);

        public static UInt32 DLLSetLabelCaption_(int reportHandle, int komponentaIndex, string Caption)
        {
            UInt32 ret = 0;//ERROR

            if (Caption == null)
            {
                MessageBox.Show("String Caption je null.\nC# Metoda: DLLSetLabelCaption_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(Caption); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetLabelCaption(reportHandle, komponentaIndex, Caption.Length, ptr);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLCopyLabelParametriToReport([In] int reportHandle);


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLUbaciRedakUTableOstaliParametri([In] int reportHandle,
                                                                        [In] int IDRow,
                                                                        [In] int IDParentRow,
                                                                        [In] int strLengthNazivKontrole,
                                                                        [In] IntPtr pStrNazivKontrole,
                                                                        [In] int strLengthNazivParametra,
                                                                        [In] IntPtr pStrNazivParametra,
                                                                        [In] int strLengthValueString,
                                                                        [In] IntPtr pStrValueString,
                                                                        [In] int valueInt,
                                                                        [In] double valueDouble,
                                                                        [In] bool valueBool);


        public static UInt32 DLLUbaciRedakUTableOstaliParametri_(int reportHandle,
                                                                 int IDRow,
                                                                 int IDParentRow,
                                                                 string nazivKontrole,
                                                                 string nazivParametra,
                                                                 string valueString,
                                                                 int valueInt,
                                                                 double valueDouble,
                                                                 bool valueBoolean)
        {
            UInt32 ret = 0;//ERROR

            if (nazivKontrole == null)
            {
                MessageBox.Show("String nazivKontrole je null.\nC# Metoda: DLLUbaciRedakUTableOstaliParametri_");
                return 0;
            }
            if (nazivParametra == null)
            {
                MessageBox.Show("String nazivParametra je null.\nC# Metoda: DLLUbaciRedakUTableOstaliParametri_");
                return 0;
            }
            if (valueString == null)
            {
                MessageBox.Show("String valueString je null.\nC# Metoda: DLLUbaciRedakUTableOstaliParametri_");
                return 0;
            }

            Byte[] bbNazivKontrole;
            bbNazivKontrole = Encoding.Unicode.GetBytes(nazivKontrole); //N characters * 2               
            Byte[] bbNazivParametra;
            bbNazivParametra = Encoding.Unicode.GetBytes(nazivParametra); //N characters * 2 
            Byte[] bbValueString;
            bbValueString = Encoding.Unicode.GetBytes(valueString); //N characters * 2 

            unsafe
            {
                fixed (byte* pNazivKontrole = bbNazivKontrole)
                {
                    fixed (byte* pNazivParametra = bbNazivParametra)
                    {
                        fixed (byte* pValueString = bbValueString)
                        {
                            IntPtr ptrNazivKontrole = (IntPtr)pNazivKontrole;
                            IntPtr ptrNazivParametra = (IntPtr)pNazivParametra;
                            IntPtr ptrValueString = (IntPtr)pValueString;

                            ret = ReportBuilderDll.DLLUbaciRedakUTableOstaliParametri(reportHandle,
                                                                              IDRow,
                                                                              IDParentRow,
                                                                              nazivKontrole.Length, ptrNazivKontrole,
                                                                              nazivParametra.Length, ptrNazivParametra,
                                                                              valueString.Length, ptrValueString,
                                                                              valueInt,
                                                                              valueDouble,
                                                                              valueBoolean);
                        }
                    }
                }
            }

            return ret;
        }

        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLCreateDatasetPodaci([In] int reportHandle);


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLInitNewRowInDataTablePodaci([In] int reportHandle);


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetCellValueStringPodaci([In] int reportHandle,
                                                                 [In] int strLengthColumnName,
                                                                 [In] IntPtr pStrColumnName,
                                                                 [In] int strLengthValue,
                                                                 [In] IntPtr pStrValue);

        public static UInt32 DLLSetCellValueStringPodaci_(int reportHandle,
                                                          string columnName,
                                                          string value)
        {
            UInt32 ret = 0;//ERROR

            string s1 = columnName;
            string s2 = value;

            if (s1 == null)
            {
                MessageBox.Show("String columnName je null.\nC# Metoda: DLLSetCellValueStringPodaci_");
                return 0;
            }

            if (s2 == null)
            {
                MessageBox.Show("String value je null.\nC# Metoda: DLLSetCellValueStringPodaci_");
                return 0;
            }

            Byte[] bb1;
            bb1 = Encoding.Unicode.GetBytes(s1); //N characters * 2               

            Byte[] bb2;
            bb2 = Encoding.Unicode.GetBytes(s2); //N characters * 2               

            unsafe
            {
                fixed (byte* p1 = bb1)
                {
                    fixed (byte* p2 = bb2)
                    {
                        IntPtr ptr1 = (IntPtr)p1;
                        IntPtr ptr2 = (IntPtr)p2;

                        ret = ReportBuilderDll.DLLSetCellValueStringPodaci(reportHandle, s1.Length, ptr1, s2.Length, ptr2);
                    }
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetCellValueIntegerPodaci([In] int reportHandle,
                                                           [In] int strLengthColumnName,
                                                           [In] IntPtr pStrColumnName,
                                                           [In] int value);

        public static UInt32 DLLSetCellValueIntegerPodaci_(int reportHandle,
                                                     string columnName,
                                                     int value)
        {
            UInt32 ret = 0;//ERROR

            string s = columnName;

            if (s == null)
            {
                MessageBox.Show("String columnName je null.\nC# Metoda: DLLSetCellValueIntegerPodaci_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(s); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetCellValueIntegerPodaci(reportHandle, s.Length, ptr, value);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetCellValueDoublePodaci([In] int reportHandle,
                                                                 [In] int strLengthColumnName,
                                                                 [In] IntPtr pStrColumnName,
                                                                 [In] double value);

        public static UInt32 DLLSetCellValueDoublePodaci_(int reportHandle,
                                                          string columnName,
                                                          double value)
        {
            UInt32 ret = 0;//ERROR

            string s = columnName;

            if (s == null)
            {
                MessageBox.Show("String columnName je null.\nC# Metoda: DLLSetCellValueDoublePodaci_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(s); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetCellValueDoublePodaci(reportHandle, s.Length, ptr, value);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetCellValueDateTimePodaci([In] int reportHandle,
                                                                   [In] int strLengthColumnName,
                                                                   [In] IntPtr pStrColumnName,
                                                                   [In] DateTime value);

        public static UInt32 DLLSetCellValueDateTimePodaci_(int reportHandle,
                                                            string columnName,
                                                            DateTime value)
        {
            UInt32 ret = 0;//ERROR

            string s = columnName;

            if (s == null)
            {
                MessageBox.Show("String columnName je null.\nC# Metoda: DLLSetCellValueDateTimePodaci_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(s); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetCellValueDateTimePodaci(reportHandle, s.Length, ptr, value);
                }
            }

            return ret;
        }


        [DllImport(DLLPath,
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLSetCellValueBooleanPodaci([In] int reportHandle,
                                                                  [In] int strLengthColumnName,
                                                                  [In] IntPtr pStrColumnName,
                                                                  [In] bool value);

        public static UInt32 DLLSetCellValueBooleanPodaci_(int reportHandle,
                                                           string columnName,
                                                           bool value)
        {
            UInt32 ret = 0;//ERROR

            string s = columnName;

            if (s == null)
            {
                MessageBox.Show("String columnName je null.\nC# Metoda: DLLSetCellValueBooleanPodaci_");
                return 0;
            }

            Byte[] bb;
            bb = Encoding.Unicode.GetBytes(s); //N characters * 2               

            unsafe
            {
                fixed (byte* p = bb)
                {
                    IntPtr ptr = (IntPtr)p;

                    ret = ReportBuilderDll.DLLSetCellValueBooleanPodaci(reportHandle, s.Length, ptr, value);
                }
            }

            return ret;
        }

        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLWriteRowInDataTablePodaci([In] int reportHandle);


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraTablicePodaci([In] int reportHandle);


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraTabliceOstaliParametri([In] int reportHandle);


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLPipelineON([In] int reportHandle);


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLFilteringON([In] int reportHandle);


        [DllImport(DLLPath,
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLPrintReport();




        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLPridruziEventeZaOstaliParametri([In] int reportHandle);


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetParamReportString([In] int reportHandle,

                                                             [In] int strLengthNazivKontrole,
                                                             [In] IntPtr pStrNazivKontrole,

                                                             [In] int strLengthNazivParametra,
                                                             [In] IntPtr pStrNazivParametra,

                                                             [In] int maxSizeBytesValue,
                                                             [Out] IntPtr pStrLengthValue,
                                                             [Out] IntPtr pStrValue);

        public static UInt32 DLLGetParamReportString_(int reportHandle,
                                                      string nazivKontrole,
                                                      string nazivParametra,
                                                      out string valueString)
        {
            UInt32 x = 0;//ERROR

            string s1 = nazivKontrole;
            string s2 = nazivParametra;
            valueString = "";

            if (s1 == null)
            {
                MessageBox.Show("String nazivKontrole je null.\nEXE Metoda: DLLGetParamReportString_");
                return 0;
            }

            if (s2 == null)
            {
                MessageBox.Show("String nazivParametra je null.\nEXE Metoda: DLLGetParamReportString_");
                return 0;
            }

            Byte[] bb1;
            bb1 = Encoding.Unicode.GetBytes(s1); //N characters * 2               

            Byte[] bb2;
            bb2 = Encoding.Unicode.GetBytes(s2); //N characters * 2                          

            unsafe
            {

                fixed (byte* p1 = bb1)
                {
                    fixed (byte* p2 = bb2)
                    {
                        fixed (byte* buffer3 = new byte[4])
                        {
                            IntPtr pStrLengthValue = (IntPtr)buffer3;

                            int maxSizeBytesValue = 1000;
                            fixed (byte* buffer4 = new byte[maxSizeBytesValue])
                            {
                                IntPtr pStrValue = (IntPtr)buffer4;

                                IntPtr ptr1 = (IntPtr)p1;
                                IntPtr ptr2 = (IntPtr)p2;

                                x = ReportBuilderDll.DLLGetParamReportString(reportHandle,
                                                                     s1.Length, ptr1,
                                                                     s2.Length, ptr2,
                                                                     maxSizeBytesValue,
                                                                     pStrLengthValue,
                                                                     pStrValue
                                                                     );
                                String Value;

                                int strLengthValue;
                                strLengthValue = Marshal.ReadInt32(pStrLengthValue);
                                Value = Marshal.PtrToStringUni(pStrValue, strLengthValue);

                                valueString = Value;
                            }
                        }
                    }
                }
            }

            return x;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetParamReportInteger([In] int reportHandle,

                                                              [In] int strLengthNazivKontrole,
                                                              [In] IntPtr pStrNazivKontrole,

                                                              [In] int strLengthNazivParametra,
                                                              [In] IntPtr pStrNazivParametra,

                                                              [Out] IntPtr pIntValue
                                                              );

        public static UInt32 DLLGetParamReportInteger_(int reportHandle,
                                                      string nazivKontrole,
                                                      string nazivParametra,
                                                      out int valueInteger)
        {
            UInt32 x = 0;//ERROR

            string s1 = nazivKontrole;
            string s2 = nazivParametra;
            valueInteger = 0;

            if (s1 == null)
            {
                MessageBox.Show("String nazivKontrole je null.\nEXE Metoda: DLLGetParamReportInteger_");
                return 0;
            }

            if (s2 == null)
            {
                MessageBox.Show("String nazivParametra je null.\nEXE Metoda: DLLGetParamReportInteger_");
                return 0;
            }

            Byte[] bb1;
            bb1 = Encoding.Unicode.GetBytes(s1); //N characters * 2               

            Byte[] bb2;
            bb2 = Encoding.Unicode.GetBytes(s2); //N characters * 2                          

            unsafe
            {

                fixed (byte* p1 = bb1)
                {
                    fixed (byte* p2 = bb2)
                    {
                        fixed (byte* buffer3 = new byte[4])
                        {
                            IntPtr pIntValue = (IntPtr)buffer3;

                            IntPtr ptr1 = (IntPtr)p1;
                            IntPtr ptr2 = (IntPtr)p2;


                            x = ReportBuilderDll.DLLGetParamReportInteger(reportHandle,
                                                                  s1.Length, ptr1,
                                                                  s2.Length, ptr2,
                                                                  pIntValue
                                                                  );


                            int Value;
                            Value = Marshal.ReadInt32(pIntValue);

                            valueInteger = Value;
                            //MessageBox.Show("EXE DLLGetParamReportInteger_\nValue=" + Value.ToString());
                        }
                    }
                }
            }

            return x;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetParamReportDouble([In] int reportHandle,

                                                              [In] int strLengthNazivKontrole,
                                                              [In] IntPtr pStrNazivKontrole,

                                                              [In] int strLengthNazivParametra,
                                                              [In] IntPtr pStrNazivParametra,

                                                              [Out] IntPtr pDoubleValue
                                                              );

        public static UInt32 DLLGetParamReportDouble_(int reportHandle,
                                                      string nazivKontrole,
                                                      string nazivParametra,
                                                      out double valueDouble)
        {
            UInt32 x = 0;//ERROR

            string s1 = nazivKontrole;
            string s2 = nazivParametra;
            valueDouble = 0.0;

            if (s1 == null)
            {
                MessageBox.Show("String nazivKontrole je null.\nEXE Metoda: DLLGetParamReportDouble_");
                return 0;
            }

            if (s2 == null)
            {
                MessageBox.Show("String nazivParametra je null.\nEXE Metoda: DLLGetParamReportDouble_");
                return 0;
            }

            Byte[] bb1;
            bb1 = Encoding.Unicode.GetBytes(s1); //N characters * 2               

            Byte[] bb2;
            bb2 = Encoding.Unicode.GetBytes(s2); //N characters * 2                          

            unsafe
            {

                fixed (byte* p1 = bb1)
                {
                    fixed (byte* p2 = bb2)
                    {
                        fixed (byte* buffer3 = new byte[8])
                        {
                            IntPtr pDoubleValue = (IntPtr)buffer3;

                            IntPtr ptr1 = (IntPtr)p1;
                            IntPtr ptr2 = (IntPtr)p2;


                            x = ReportBuilderDll.DLLGetParamReportDouble(reportHandle,
                                                                  s1.Length, ptr1,
                                                                  s2.Length, ptr2,
                                                                  pDoubleValue
                                                                  );


                            double Value;

                            byte[] bb = new byte[8];
                            Marshal.Copy(pDoubleValue, bb, 0, 8);
                            Value = BitConverter.ToDouble(bb, 0);

                            valueDouble = Value;
                            //MessageBox.Show("EXE DLLGetParamReportDouble_\nValue=" + Value.ToString());
                        }
                    }
                }
            }

            return x;
        }


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        private static extern UInt32 DLLGetParamReportBoolean([In] int reportHandle,

                                                              [In] int strLengthNazivKontrole,
                                                              [In] IntPtr pStrNazivKontrole,

                                                              [In] int strLengthNazivParametra,
                                                              [In] IntPtr pStrNazivParametra,

                                                              [Out] IntPtr pBooleanValue
                                                              );

        public static UInt32 DLLGetParamReportBoolean_(int reportHandle,
                                                      string nazivKontrole,
                                                      string nazivParametra,
                                                      out bool valueBoolean)
        {
            UInt32 x = 0;//ERROR

            string s1 = nazivKontrole;
            string s2 = nazivParametra;
            valueBoolean = false;

            if (s1 == null)
            {
                MessageBox.Show("String nazivKontrole je null.\nEXE Metoda: DLLGetParamReportBoolean_");
                return 0;
            }

            if (s2 == null)
            {
                MessageBox.Show("String nazivParametra je null.\nEXE Metoda: DLLGetParamReportBoolean_");
                return 0;
            }

            Byte[] bb1;
            bb1 = Encoding.Unicode.GetBytes(s1); //N characters * 2               

            Byte[] bb2;
            bb2 = Encoding.Unicode.GetBytes(s2); //N characters * 2                          

            unsafe
            {

                fixed (byte* p1 = bb1)
                {
                    fixed (byte* p2 = bb2)
                    {
                        fixed (byte* buffer3 = new byte[1])
                        {
                            IntPtr pBooleanValue = (IntPtr)buffer3;

                            IntPtr ptr1 = (IntPtr)p1;
                            IntPtr ptr2 = (IntPtr)p2;


                            x = ReportBuilderDll.DLLGetParamReportBoolean(reportHandle,
                                                                  s1.Length, ptr1,
                                                                  s2.Length, ptr2,
                                                                  pBooleanValue
                                                                  );


                            bool Value;
                            byte b;

                            b = Marshal.ReadByte(pBooleanValue);

                            Value = System.Convert.ToBoolean(b);

                            //byte[] bb = new byte[1];
                            //Marshal.Copy(pBooleanValue, bb, 0, 1);
                            //Value = BitConverter.ToBoolean(bb, 0);

                            valueBoolean = Value;
                            //MessageBox.Show("EXE DLLGetParamReportBoolean_\nValue=" + Value.ToString());
                        }
                    }
                }
            }

            return x;
        }






        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraLabela();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraDBTextova();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraDBMemoa();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraDBRichTextova();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraDBImagea();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraDBCheckBoxeva();

        //[DllImport(DLLPath,
        //    CallingConvention = CallingConvention.StdCall,
        //    CharSet = CharSet.Unicode)]
        //public static extern UInt32 DLLProvjeraDBCalcova();

        //[DllImport(DLLPath,
        //    CallingConvention = CallingConvention.StdCall,
        //    CharSet = CharSet.Unicode)]
        //public static extern UInt32 DLLProvjeraRegiona();


        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraTablice();

        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProvjeraKomponenti([In] int reportHandle);

        [DllImport(DLLPath,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern UInt32 DLLProba([In] int reportHandle);
    }
}
