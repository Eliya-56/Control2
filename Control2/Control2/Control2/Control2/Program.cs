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

    enum SizeUnits
    {
        B,
        MB,
        GB,
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

            List textFiles = new List();
            List imageFiles = new List();
            List movieFiles = new List();

            for (int i = 0; i < strsFile.Length; i++)
            {
                strFiles[i] = new ParseStrFile(strsFile[i]);
                if(strFiles[i].Type == FileType.Text)
                {
                    textFiles.Add(new TextFileDesc(strFiles[i]));
                }
                else if (strFiles[i].Type == FileType.Image)
                {
                    imageFiles.Add(new ImageFileDesc(strFiles[i]));
                }
                else if (strFiles[i].Type == FileType.Movie)
                {
                    movieFiles.Add(new MovieFileDesc(strFiles[i]));
                }
            }

            Console.WriteLine("Text files:");
            foreach (var textFile in textFiles)
            {
                ((TextFileDesc)textFile).ShowDesc();
            }
            Console.WriteLine();
            Console.WriteLine("Movies:");
            foreach (var movieFile in movieFiles)
            {
                ((MovieFileDesc)movieFile).ShowDesc();
            }
            Console.WriteLine();
            Console.WriteLine("Images:");
            foreach (var imageFile in imageFiles)
            {
                ((ImageFileDesc)imageFile).ShowDesc();
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
        public string FileProp;

        public ParseStrFile(string strFile)
        {
            FileProp = "";
            string[] strProp = strFile.Split(new string[] { ";", ":", "(",")" }, StringSplitOptions.RemoveEmptyEntries);
            GetType(strProp[0]);
            GetNameExtension(strProp[1]);
            GetSize(strProp[2]);


            int i = 3;
            while (i < strProp.Length)
            {
                FileProp += strProp[i] + ";";
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

            SizeUnits tempUnit;
            switch (units)
            {
                case "B":
                case "b":
                case "byte":
                    tempUnit = SizeUnits.B;
                    break;
                case "MB":
                case "mb":
                case "megabyte":
                    tempUnit = SizeUnits.MB;
                    break;
                case "GB":
                case "gb":
                case "gigabyte":
                    tempUnit = SizeUnits.B;
                    break;
                default:
                    tempUnit = SizeUnits.MB;
                    break;

            }
            this.Size = new FileSize(int.Parse(value), tempUnit);
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
        public SizeUnits Units { get; set; }
        public FileSize(int Value, SizeUnits Units)
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
        public int hours { get; set; }
        public int mins { get; set; }
        public Durability(int hours, int mins)
        {
            this.hours = hours;
            this.mins = mins;
        }
    }

    class FileDesc : IComparable
    {
        public string Name { get; set; }
        public string Extension { get; protected set; }
        public FileType Type { get; protected set; }
        public FileSize Size { get; protected set; }


        public FileDesc(ParseStrFile str)//string Name, FileType Type, string Extension, FileSize Size)
        {
            this.Name = str.Name;
            this.Type = str.Type;
            this.Extension = str.Extension;
            this.Size = str.Size;
        }

        public virtual void ShowDesc()
        {
            Console.WriteLine("     " + Name + Extension);
            Console.WriteLine($"         Extension: {this.Extension}");
            Console.WriteLine($"         Size: {this.Size.Value} {this.Size.Units}");
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            FileDesc otherFileDesc = obj as FileDesc;

            if ((int)otherFileDesc.Size.Units == (int)this.Size.Units)
            {
                if (otherFileDesc.Size.Value > this.Size.Value)
                {
                    return 1;
                }
                else if (otherFileDesc.Size.Value < this.Size.Value)
                {
                    return -1;
                }
                else
                    return 0;
            }
            else if ((int)otherFileDesc.Size.Units > (int)this.Size.Units)
            {
                return 1;
            }
            else
                return 0;
        }
    }

    class TextFileDesc : FileDesc
    {
        public string Content { get; protected set; }

        public TextFileDesc(ParseStrFile str)
            :base(str)
        {
            string[] tmp = str.FileProp.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            this.Content = tmp[0];
        }

        public override void ShowDesc()
        {
            base.ShowDesc();
            Console.WriteLine($"         Content: " + this.Content);
        }

    }

    class ImageFileDesc : FileDesc
    {
        public ImageResolution Resolution { get; private set; }

        public ImageFileDesc(ParseStrFile str)
            :base(str)
        {
            string[] tmp = str.FileProp.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            GetImageResolution(tmp[0]);
        }

        public override void ShowDesc()
        {
            base.ShowDesc();
            Console.WriteLine($"         Resolution: {this.Resolution.Hor}x{this.Resolution.Ver}");
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

        public MovieFileDesc(ParseStrFile str)
            :base(str)
        {
            string[] tmp = str.FileProp.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            GetDurability(tmp[1]);
        }

        public override void ShowDesc()
        {
            base.ShowDesc();
            Console.WriteLine($"         Durability: " + this.Durability.hours + "m" + this.Durability.mins + "m");
        }

        private void GetDurability(string Durability)
        {
            string[] temp = Durability.Split(new string[] { "h","m" }, StringSplitOptions.RemoveEmptyEntries);
            this.Durability = new Durability(int.Parse(temp[0]), int.Parse(temp[1]));
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
