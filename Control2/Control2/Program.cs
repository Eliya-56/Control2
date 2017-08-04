using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyList;

namespace Control2
{
    enum FileType
    {
        Text,
        Image,
        Movie,
        UnknownType,
    }

    class Program
    {
        static void Main(string[] args)
        {
            string text = @"Text:file.txt(6B);Some string content
Image:img.bmp(19MB);1920x1080
Text:data.txt(12B);Another string
Text:data1.txt(7B);Yet another string
Movie:logan.2017.mkv(19GB);1920x1080; 2h12m";

            string[] strsFile = text.Split(new string[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            ParseStrFile[] strFiles = new ParseStrFile[strsFile.Length];
            FileDesc[] files = new FileDesc[strsFile.Length];

            for (int i = 0; i < strsFile.Length; i++)
            {
                strFiles[i] = new ParseStrFile(strsFile[i]);
                if(strFiles[i].Type == FileType.Text)
                {
                    files[i] = new TextFileDesc(strFiles[i].Name, strFiles[i].Type, strFiles[i].Extension, strFiles[i].Size, (string)strFiles[i].FileProp[0]);
                }
                if (strFiles[i].Type == FileType.Image)
                {
                    files[i] = new ImageFileDesc(strFiles[i].Name, strFiles[i].Type, strFiles[i].Extension, strFiles[i].Size, (string)strFiles[i].FileProp[0]);
                }
                if (strFiles[i].Type == FileType.Movie)
                {
                    files[i] = new MovieFileDesc(strFiles[i].Name, strFiles[i].Type, strFiles[i].Extension, strFiles[i].Size, (string)strFiles[i].FileProp[0], (string)strFiles[i].FileProp[1]);
                }
            }

            //TextFileDesc[] txtFiles = 

            foreach (var file in files)
            {
                file.ShowDesc();
            }


            Console.ReadLine();
        }
    }

    class ParseStrFile
    {
        public FileType Type { get; private set; }
        public string Name { get; private set; }
        public string Extension { get; private set; }
        public FileSize Size { get; private set; }
        public List FileProp;

        public ParseStrFile(string strFile)
        {
            FileProp = new List();
            string[] strProp = strFile.Split(new string[] { ";", ":", "(",")" }, StringSplitOptions.RemoveEmptyEntries);
            GetType(strProp[0]);
            GetNameExtension(strProp[1]);
            GetSize(strProp[2]);


            int i = 3;
            while (i < strProp.Length)
            {
                FileProp.Add(strProp[i]);
                i++;
            }
        }

        public void GetNameExtension(string NameExt)
        {
            int index = NameExt.LastIndexOf(".");
            string ext = "";
            string name = "";
            for (int i = 0; i < NameExt.Length; i++)
            {
                if(i < index)
                {
                    name += NameExt[i];
                }
                else
                {
                    ext += NameExt[i];
                }
            }
            Name = name;
            Extension = ext;
        }

        private void GetSize(string Size)
        {
            string value = "";
            string units = "";

            for (int i = 0; i < Size.Length; i++)
            {
                if (Char.IsDigit(Size[i]))
                {
                    value += Size[i];
                } else
                {
                    units += Size[i];
                }
            }
            this.Size = new FileSize(int.Parse(value), units);
        }

        private void GetType(string Type)
        {
            if(Type == "Text")
            {
                this.Type = FileType.Text;
            }
            else if(Type == "Image")
            {
                this.Type = FileType.Image;
            }
            else if(Type == "Movie")
            {
                this.Type = FileType.Movie;
            }
            else
            {
                this.Type = FileType.UnknownType;
            }
        }
    }

    struct FileSize
    {
        public int Value { get; set; }
        public string Units { get; set; }
        public FileSize(int Value, string Units)
        {
            this.Value = Value;
            this.Units = Units;
        }
    }

    struct ImageResolution
    {
        public int Hor { get; set; }
        public int Ver { get; set; }
        public ImageResolution(int Hor, int Ver)
        {
            this.Hor = Hor;
            this.Ver = Ver;
        }
    }

    struct Durability
    {
        int hours;
        int mins;
        public Durability(int hours, int mins)
        {
            this.hours = hours;
            this.mins = mins;
        }
    }

    class FileDesc
    {
        public string Name { get; set; }
        public string Extension { get; protected set; }
        public FileType Type { get; protected set; }
        public FileSize Size { get; protected set; }


        public FileDesc(string Name, FileType Type, string Extension, FileSize Size)
        {
            this.Name = Name;
            this.Type = Type;
            this.Extension = Extension;
            this.Size = Size;
        }

        public virtual void ShowDesc()
        {
            Console.WriteLine(Name + Extension);
            Console.WriteLine($"    Extension: {this.Extension}");
            Console.WriteLine($"    Size: {this.Size.Value} {this.Size.Units}");
        }
    }

    class TextFileDesc : FileDesc
    {
        public string Content { get; protected set; }

        public TextFileDesc(string Name, FileType Type, string Extension, FileSize Size, string Content)
            :base(Name, Type, Extension, Size)
        {
            this.Content = Content;
        }

        public override void ShowDesc()
        {
            base.ShowDesc();
            Console.WriteLine($"    Content: " + this.Content);
        }

    }

    class ImageFileDesc : FileDesc
    {
        public ImageResolution Resolution { get; private set; }

        public ImageFileDesc(string Name, FileType Type, string Extension, FileSize Size, string Resolution)
            :base(Name, Type, Extension, Size)
        {
            GetImageResolution(Resolution);
        }

        public override void ShowDesc()
        {
            base.ShowDesc();
            Console.WriteLine($"    Resolution: {this.Resolution.Hor}x{this.Resolution.Ver}");
        }

        private void GetImageResolution(string res)
        {
            string[] temp = res.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
            Resolution = new ImageResolution(int.Parse(temp[0]), int.Parse(temp[1]));
        }
    }

    class MovieFileDesc: ImageFileDesc
    {
        public Durability Durability { get; set; }

        public MovieFileDesc(string Name, FileType Type, string Extension, FileSize Size, string Resolution, string Durability)
            :base(Name, Type, Extension, Size, Resolution)
        {
            //this.Durability = Durability;
        }

        public override void ShowDesc()
        {
            base.ShowDesc();
            Console.WriteLine($"    Durability: " + this.Durability);
        }

        private void GetDurability(string Durability)
        {
            string[] temp = Durability.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            this.Durability = new Durability(int.Parse(temp[0]), int.Parse(temp[0]));
        }

    }

    class List : IEnumerable
    {
        private object[] MyList;
        public int Length
        {
            get
            {
                return MyList.Length;
            }

        }

        public object this[int index]
        {
            get
            {
                if (index > Length - 1)
                {
                    Console.WriteLine("Out of range ");
                    return null;
                }
                return MyList[index];
            }
            set
            {
                MyList[index] = value;
            }
        }
        public List()
        {
            MyList = new object[0];
        }

        public void Add(object item)
        {
            object[] temp = new object[MyList.Length + 1];
            for (int i = 0; i < MyList.Length; i++)
            {
                temp[i] = MyList[i];
            }
            temp[MyList.Length] = item;
            MyList = temp;
        }

        public void Insert(int index, object item)
        {
            if (index > Length - 1)
            {
                Console.WriteLine("Out of range ");
                return;
            }

            object[] temp = new object[MyList.Length + 1];
            for (int i = 0; i < index; i++)
            {
                temp[i] = MyList[i];
            }
            temp[index] = item;
            for (int i = index; i < MyList.Length; i++)
            {
                temp[i + 1] = MyList[i];
            }
            MyList = temp;
        }

        public void Remove(object item)
        {
            object[] temp = new object[MyList.Length - 1];
            for (int i = 0, j = 0; i < MyList.Length; i++, j++)
            {
                if (MyList[i].Equals(item))
                {
                    i++;
                    if (j == temp.Length)
                    {
                        MyList = temp;
                        return;
                    }
                } else if (j == temp.Length && j == i)
                {
                    return;
                }
                temp[j] = MyList[i];
            }
            MyList = temp;
        }

        public void RemoveAt(int index)
        {
            object[] temp = new object[MyList.Length - 1];
            for (int i = 0, j = 0; j < temp.Length; i++, j++)
            {
                if (i == index)
                {
                    i++;
                }
                temp[j] = MyList[i];
            }
            MyList = temp;
        }

        public void Clear()
        {
            MyList = new object[0];
        }

        public bool Contains(object item)
        {
            for (int i = 0; i < MyList.Length; i++)
            {
                if (MyList[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public int IndexOf(object item)
        {
            for (int i = 0; i < MyList.Length; i++)
            {
                if (MyList[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public object[] ToArray()
        {
            object[] output = new object[Length];
            for (int i = 0; i < MyList.Length; i++)
            {
                output[i] = MyList[i];
            }
            return output;
        }

        public void Reverse()
        {
            for (int i = 0; i < MyList.Length / 2; i++)
            {
                object temp;
                temp = MyList[i];
                MyList[i] = MyList[MyList.Length - 1 - i];
                {
                    MyList[MyList.Length - 1 - i] = temp;
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return MyList.GetEnumerator();
        }
    }

}
