using System;

namespace Proje1
{
    internal class Program
    {
        static Random rand = new Random(); //random degerler kullanmak icin degisken tanimlaniyor

        public static void Main(string[] args)
        {
          //nx2'lik matris uretmek icin satir sayisini kullanicidan aliyoruz

            Console.WriteLine("Uretmek istediginiz nx2'lik matrisin uzunlugunu giriniz:");
            int n = int.Parse(Console.ReadLine());

           //genisligi ve yuksekligi 100 olan, uzunlugu n olan yazdigimiz metot yardimiyla random matris olusturuyoruz
           
            int width = 100;
            int height = 100;
            double[,] randomMatrix = RandomMatrix(n, width, height);
            
            //olusturdugumuz matrisdeki her bir sutunun satirlarinda yerlesen x ve y degerlerini ekrana yazdiriyoruz
            
            Console.WriteLine("n="+ n +" width=100, height=100 değerleri için x ve y değerleri:\n");
            Console.WriteLine("X\t\tY\n--\t\t--");
            for (int i = 0; i < n; i++)

            {
                Console.WriteLine("{0}\t\t{1}", randomMatrix[i, 0].ToString("n1"), randomMatrix[i, 1].ToString("n1"));
            }


            //Kendisine verilen nx2 noktalar matrisini nxn’lik uzaklık matrisine çevirerek ekrana tablo yazdiriliyor
            //metodun ne ise yaradigi ve nasil yapildigi ile ilgili aciklama asagida yapilmistir
            
            double[,] distanceMatrix = DistanceMatrix(randomMatrix);

            Console.WriteLine("\t\t\t\t\t\t\t\tDISTANCE MATRIX");

            //for dongusu ile tablonun sutunlari numaralanıyor
            for (int i = 0; i < n; i++)
            {
                Console.Write("{0,7:N0}", i);
            }

            Console.WriteLine();

            //dışdaki for dongusu ile tablonun satırları numaralanıyor
            for (int i = 0; i < n; i++)
            {
                Console.Write("{0,2}", i);
                
                //içdeki for döngüsü ile sırasıyla her bir satıra uzaklik matrisindeki elemanlar yazılıyor
                //yani iki nokta arasındaki uzaklık hesaplanarak tablonun uygun satır ve sütununa yazdırılıyor
                for (int j = 0; j < n; j++)
                {

                    Console.Write("{0,7}", distanceMatrix[i, j].ToString("n1"));
                }

                Console.WriteLine();

            }
            
            // Bu metodun ne ise yaradigi ve nasil yapildigi ile ilgili aciklama asagida yapilmistir
            NearestNeighbor(distanceMatrix);

        }


        
        //Genişliği ve (height) verilen 2 boyutlu alan içerinde (yani nx2lik) matriste n adet rastgele nokta üreten  metod
        //parametre olarak üretmek istediğimiz matrisin satır sayısını ve random olarak üreteceğimiz değerlerin sınır degerlerini,
        //yani 2 boyutlu alanın genişliğini ve yüksekliğini gönderiyoruz 
        
        public static double[,] RandomMatrix(int n, int width, int height)
        {

            //nx2 lik matris yaratıyoruz
            double[,] rm = new double[n, 2];
            
            //for döngüsü ile sırasıyla matrisin satırlarına ğnceden verilmiş yükseklik ve genişlik değerleri arasında random degerler atanıyor 
            for (int i = 0; i < n; i++)

            {
                //NextDouble 0,1 arasinda urettigi icin,(1 dahil degil) sinir degerlerine 1 ekleyerek olusan degeri carpiyoruz
                rm[i, 0] = rand.NextDouble()*(width+1);
                rm[i, 1] = rand.NextDouble()*(height+1);
            }

            //üretilen matrisi döndürüyoruz
            return rm;

        }

        
        
        //Kendisine verilen nx2 noktalar matrisini nxn lik uzaklık matrisine çeviren metot
        //parametre olarak yukarda ürettiğimiz nx2 lik matrisi veriyoruz 
        public static double[,] DistanceMatrix(double[,] matrix)
        {

            int a = matrix.GetLength(0);
            //noktalar arasında uzaklık değerlerini atamak yeni bir axa lık matris üretiliyor
            double[,] dm = new double[a,a];
            
            //sırasıyla her satırdaki her noktayla yerde kalan diğer noktalar arasındakı uzaklık hesaplanıyor
            for (int i = 0; i < a; i++)
            {

                //dm[i,j] = dm[j,i] eşitliği sağlandığından, diğer noktalara göre uzaklığı hesaplanacak noktanın
                //o noktadan sonrakı noktalar ile uzaklık hesaölanıyor
                for (int j = i; j < a; j++)
                {
                    //İki boyutlu uzayda iki nokta arasındaki Öklid uzaklığı aşağıdaki formüle göre hesaplanıyor
                    
                    dm[i, j] = Math.Sqrt(Math.Pow(matrix[j, 0] - matrix[i, 0], 2) +
                                         Math.Pow(matrix[j, 1] - matrix[i, 1], 2));
                    
                    //DM[i,j]=DM[j,i] eşitliği sağlanıyor
                    dm[j, i] = dm[i, j];

                }
            }

            //ürettiğimiz uzaklık matrisini dündürüyoruz
            return dm;


        }

        
        //rastgele bir noktadan başlayarak tüm noktaları en yakın komşu yöntemine (nearest neighbor) göre dolaşan metod
        public static void NearestNeighbor(double[,] matrix)
        {
            //soruda bizden 10 farklı tur için bilgiler istendiğinden turSayısını 10 olarak kabul ettim
            int turSayi = 10;

            //her bir tur için istenen bilgiler ekrana yazdırılacaktır
            for (int i = 0; i < turSayi; i++)
            {
                Console.WriteLine("\n" +(i + 1) + ".TUR:");
                
                //iki nokta arasındaki en kısa mesafeni ve noktaları kendinde tutan nx3 lük bir matris
                //dolaşılan noktanın bir daha dolaşılmamasi için 
                double[,] yollar = new double[matrix.GetLength(0), 3];

                //gidilen toplam yol için değişken
                double topYol = 0;

                //diğer noktaları dolaşmağa rastgele bir noktadan başlayacağı için random baslanğıc numarası tanımlıyoruz
                int randomSayi = rand.Next(0, matrix.GetLength(0));

                
                for (int k = 0; k < matrix.GetLength(0); k++)
                {

                    double minDis = 1000;
                    //sırasıyla gittiği noktaları (yani bir noktaya en yakın olan noktaları) matriste tutuyoruz
                    yollar[k, 1] = randomSayi;

                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {

                        //bir noktanın kendisiyle uzaklığı 0 olduğu için o durum olmayan hallere bakıyoruz
                        if (randomSayi != j)
                        {
                            //seçilen başlanğıc noktasına en yakın uzaklıkta olan nokta buluyoruz
                            if (matrix[randomSayi, j] < minDis)
                            {
                                Boolean flag = true;

                                //bir noktadan diğer noktaya daha önceden gidilib gidilmediğini kontrol ediyoruz
                                //bir noktadan 2 kere geçmeyi engelliyoruz
                                for (int l = 0; l < k; l++)
                                {
                                    
                                    if (j == (int) yollar[l, 2] || j == (int) yollar[l, 1])
                                    {
                                        flag = false;
                                    }
                                }


                                //eğer yukardakı koşullar sağlanmıyorsa, yani noktaya en yakın olan nokta tur boyunca daha önce dolaşılmadıysa 
                                //iki nokta arasındakı min yolu ve hangi nokta olduğu değerini matrise atıyoruz
                                if (flag)
                                {

                                    minDis = matrix[randomSayi, j];

                                    yollar[k, 0] = minDis;

                                    yollar[k, 2] = j;

                                }

                            }

                        }
                    }

                    //yukardakı işlemler bittikten sonra önceki noktaya en yakın nokta için aynı işlemleri tekrarlıyoruz, bunun için seçilen 
                    //random sayıyı ona en yakın nokta olacak şekilde değiştiriyoruz
                    randomSayi = (int) yollar[k, 2];
                    
                    //Turun toplam yol uzunluğu bulunuyor
                    topYol += yollar[k, 0];

                }
                

                //İlgili turda sırayla hangi numaralı noktalara uğradığI ekrana yazılıyor
                Console.WriteLine("Uğradığı Noktalar:");
                for (int j = 0; j < yollar.GetLength(0); j++)
                {
                    Console.Write(yollar[j, 1] + " ");

                }

                //Turun toplam yol uzunluğu ekrana yazılıyor
                Console.WriteLine("\nToplam gidilen yol: " + topYol.ToString("n1"));


            }
        }
    }
}