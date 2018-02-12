using System.Collections;
 
namespace Stk.DbSql
{
    public class TransactionSetCollection : IEnumerable
    {
  
            public ArrayList Arr = new ArrayList();
            public TransactionSet GetTransactionSetList(int i)
            {
                return (TransactionSet)Arr[i]; //แสดงข้อมูลในตำแหน่ง ที่ i
            }
            public void AddTransactionSet(TransactionSet p)
            {
                Arr.Add(p); // เพื่ม ข้อมูลชนิด object ของคลาส Person เข้าไปใน ArrayList
            }
            public int CountTransactionSet
            {
                get
                {
                    return Arr.Count; // นับจำนวนสมาชิกใน ArrayList
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return Arr.GetEnumerator(); //สำหรับการวนลูปโดยใช้ foreach
            }
 

    }
}