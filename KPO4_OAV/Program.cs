using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using static System.Console;

namespace KPO4_OAV
{
    class Program
    {
        /*Разработать XML-формат для представления расписания учебных занятий 
        вашей группы. В расписании должна храниться информация о занятиях на 
        каждый день недели. Для каждого учебного занятия указаны название 
        предмета, аудитория, преподаватель, время начала и окончания, 
        тип занятия (лекция или практика).*/
        static bool validation = true;
        public static void Main(string[] args)
        {
            /*1.В разработанном формате представить расписание текущей недели.*/
            string path = @"C:\Users\hebgehogg\Desktop\КПО4\тесты\";
            var XMLdoc = XDocument.Load(path+"schedule.xml");

            /*2.Разработать xPath-запросы:*/
            /* /descendant-or-self::node() - //
             * child::* - *
             * attribute::* - @*
             * self::node() .
             * parent::node() - .. */

            /*a. Получить все занятия на данной неделе.*/ /*БЕЗ ПОВТОРОВ*/
            int counter = 1;
            var getlesson = XMLdoc.XPathSelectElements("//lesson").Attributes("title").Select(x => x.Value);
            WriteLine("a. Все занятия на данной неделе: ");
            foreach (var x in getlesson.Distinct()) { WriteLine($"\t{counter}. {x}"); counter++; }

            /*b. Получить все аудитории, в которых проходят занятия.*/
            var getclass = XMLdoc.XPathSelectElements("//class").Select(x => x.Value);
            Write("\nb. Все аудитории, в которых проходят занятия: ");
            foreach (var x in getclass.Distinct())
                Write(x + " ");

            /*c. Получить все практические занятия на неделе.*/
            counter = 1;
            var getpractics = XMLdoc.XPathSelectElements("//lesson[type='Практика']").Attributes("title").Select(x=>x.Value);
            WriteLine("\n\nc. Все практические занятия на неделе: ");
            foreach (var x in getpractics.Distinct()) { WriteLine($"\t{counter}. {x}"); counter++; }

            /*d. Получить все лекции, проводимые в указанной аудитории.*/
            WriteLine("\nd. Получить все лекции, проводимые в указанной аудитории.");
            WriteLine("Введите аудиторию: ");
            counter = 1;
            int _class = check();
            var getconferencia = XMLdoc.XPathSelectElements($"//lesson[type='Лекция' and class='{_class}']").Attributes("title").Select(x => x.Value);
            WriteLine($"Все лекции, проводимые в {_class} аудитории: ");
            foreach (var x in getconferencia.Distinct()) { WriteLine($"\t{counter}. {x}"); counter++; }
            if (getconferencia.Count() == 0) Write("Лекций нет в этой аудитории");

            /*e. Получить список всех преподавателей, проводящих практики в указанной аудитории.*/
            WriteLine("\n\ne. Получить список всех преподавателей, проводящих практики в указанной аудитории.");
            WriteLine("Введите аудиторию: ");
            _class = check();
            var getteachers = XMLdoc.XPathSelectElements($"//lesson[type='Практика' and class='{_class}']/teacher").Select(x => x.Value);
            WriteLine($"Cписок всех преподавателей, проводящих практики в {_class} аудитории: ");
            foreach (var x in getteachers.Distinct()) { WriteLine($"\t{counter}. {x}"); counter++; }
            if (getteachers.Count() == 0) Write("Учителя не преподают в этой аудитории");

            /*f. Получить последнее занятие для каждого дня неделели.*/
            WriteLine("\n\nf. Последнее занятия для каждого дня недели: ");
            var getday = XMLdoc.XPathSelectElements("//day");
            foreach (XElement x in getday){
                var getlast = x.XPathSelectElements("./lesson").Attributes("title").Select(y => y.Value);
                WriteLine($"\tВ {x.Attribute("name_day").Value} последняя пара: {getlast.LastOrDefault()}");
            }

            /*g. Получить общее количество занятий за всю неделю.*/
            var getcountlesson = XMLdoc.XPathEvaluate("sum(//day/@count_lesson)");
            WriteLine($"\ng. Общее количество занятий за неделю: {getcountlesson.ToString()}");

            /*3.Описать DTD (Document Type Definition) схему для разработанного формата.
             * Произвести валидацию xml - документа.*/
            WriteLine("\n3.Описать DTD(Document Type Definition) схему для разработанного формата. Произвести валидацию xml - документа.");
            /*• ATTLIST (список атрибутов) - объявляет список XML-атрибутов. Эти атрибуты определяются именем, типом данных, неявными
              значениями по умолчанию и именами любых элементов, позволяющих их использование.
              • ELEMENT - объявляет имя типа XML-элемента и его допустимые вложенные (дочерние) элементы.
              • ENTITY - объявляет специальные символьные ссылки, текстовые макросы (наподобие инструкции #define языка C/C++) и другое
              повторяющееся содержимое (наподобие инструкции #include языка C/C++).
              • NOTATION - объявляет внешнее содержимое, не относящееся к XML (например, двоичные графические данные), а также внешнее
              приложение, которое обрабатывает это содержимое. 
              • #REQUIRED - Атрибут должен присутствовать в XMLдокументе, иначе при синтаксическом разборе будет сформирована ошибка. В
              некоторых случаях, чтобы избежать возникновения ошибки, можно по желанию использовать поле defaultValue,
              поместив его непосредственно за этим ключевым словом. 
              • CDATA – атрибут содержит только символьные данные. 
              • #PCDATA - Содержимое элемента может быть анализируемыми символьными данными. */
            var criteries = new XmlReaderSettings();
            criteries.DtdProcessing = DtdProcessing.Parse;//создаю настройку
            criteries.ValidationType = ValidationType.DTD;//задаем тип
            criteries.ValidationEventHandler += new ValidationEventHandler(Valid);//обработка ошибок
            var xml_dtd = XmlReader.Create(path+"dtd_schedule.xml", criteries);
            WriteLine("Валидация документа по DTD:");
            while (xml_dtd.Read())
                if (!validation) break;
            if (validation) WriteLine("Документ валидный");

            /*4.Описать XML Schema (XSD) для разработанного формата.
             * Произвести валидацию xml - документа.*/
            /*XML-схема - это основанная на XML современная альтернатива DTD, описывающая структуру XML-документа, в том числе*/
            WriteLine("\n4.Описать XML Schema (XSD) для разработанного формата.");
            validation = true;
            var schema = new XmlSchemaSet();
            schema.Add("", XmlReader.Create(path+"schema_schedule.xsd"));
            WriteLine("Валидация документа по схеме XSD:");
            XMLdoc.Validate(schema, new ValidationEventHandler(Valid));//проверка XMLdoc
            if (validation) WriteLine("Документ валидный");

            /*5.Описать XSLT-преобразование xml-документа в текстовый вид (*.txt).*/
            WriteLine("\n5.Описать XSLT-преобразование xml-документа в текстовый вид (*.txt).");
            var xslt_to_txt = new XslCompiledTransform();
            xslt_to_txt.Load(path+ "sсhedule_txt.xsl");
            xslt_to_txt.Transform(
                XmlReader.Create(path+"dtd_schedule.xml", new XmlReaderSettings()
                    { ProhibitDtd = false }),//разрешено проходить обработку
                XmlWriter.Create(path+"XSLT_schedule.txt", new XmlWriterSettings()
                    { ConformanceLevel = ConformanceLevel.Auto }));//записывает тхт
            WriteLine("Done");

            /*6.Описать XSLT-преобразование xml-документа в html-страницу
              (расписание должно быть представлено в виде таблицы)*/
            WriteLine("\n6.Описать XSLT-преобразование xml-документа в html-страницу (расписание должно быть представлено в виде таблицы)");
            var xslt_to_html = new XslCompiledTransform();
            xslt_to_html.Load(path+"html_schedule.xsl");
            xslt_to_html.Transform(
                XmlReader.Create(path+"dtd_schedule.xml", new XmlReaderSettings()
                    { ProhibitDtd = false }),
                XmlWriter.Create(path+"XSLT_schedule.html", new XmlWriterSettings()
                    { ConformanceLevel = ConformanceLevel.Auto }));
            WriteLine("Done");

          ReadKey();
        }
        private static void Valid(object sender, ValidationEventArgs e)
        {
            validation = false;
            WriteLine($"Документ НЕ валидный: {e.Message}");
        }
        static int check()
        {
            while (true) {
                int number;
                if (int.TryParse(Console.ReadLine(), out number)) return number;
                else Console.Write("Ошибка, введите еще раз");
            }
        }
    }
}

