using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyList
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList newLList = new LinkedList();
            newLList.Add(15);
            newLList.Add(16);
            newLList.Add("fdsa");
           

            //Console.WriteLine(newList.IndexOf("ogfddun"));

            Console.ReadLine();
        }
    }

    interface ICollection : IEnumerable
    {

    }

    class List
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
            for(int i = 0; i < MyList.Length / 2; i++)
            {
                object temp;
                temp = MyList[i];
                MyList[i] = MyList[MyList.Length - 1 - i];
                {
                    MyList[MyList.Length - 1 - i] = temp;
                }
            }
        }
    }

    class Node
    {
        public object Value;
        public Node Next;
    }

    class LinkedList
    {
        private Node First;
        private Node Last;
        public int Count
        {
            get
            {
                if (First.Value == null)
                    return 0;
                else
                {
                    int count = 1;
                    Node current = First;
                    while (current.Next != null)
                    {
                        count++;
                        current = current.Next;
                    }
                    return count;
                }
            }
        }

        public LinkedList()
        {
        }

        public void Add(object item)
        {
            if(Count == 0)
            {
                First.Value = item;
                First.Next = null;
            }
            else if(Count == 1)
            {
                First.Next = new Node() { Value = item, Next = null };
                Last = First.Next;
            }
            else
            {
                Last.Next = new Node { Value = item, Next = null };
                Last = Last.Next;
            }
        }
    }
}
