# Спецификация языка разметки

Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github.

Процессору на вход подается одна строка — параграф текста. 
На выходе должен быть HTML-код этого параграфа.

Текст _окруженный с двух сторон_  одинарными символами подчерка 
должен помещаться в HTML-тег em вот так:

`Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка 
должен помещаться в HTML-тег em вот так:`

Любой символ можно экранировать, чтобы он не считался частью разметки. 
\_Вот это\_, не должно выделиться тегом \<em\>.

__Двумя символами__ — должен становиться жирным с помощью тега \<strong\>.

Внутри __двойного выделения _одинарное_ тоже__ работает.

Но не наоборот — внутри _одинарного __двойное__ не работает_.

Если рядом с выделением(_ или __) стоит цифра или подчерк - оно не является выделением.
То есть, \_1\_ не будет считаться выделением.
Также ___(3 подчерка) не будет считаться выделением.

За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.

Перед подчерками, начинающими выделение, должен быть пробельный символ(или начало строки).

Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти \_подчерки \_не считаются окончанием выделения и остаются просто символами подчерка.

После подчерков, заканчивающих выделение, должен быть пробельный символ(или конец строки).

### Гиперссылки:
Можно добавлять гиперссылки следующим образом:
\[Hi!\]\(ya.ru\)  
Это превращается в [Hi!](ya.ru)  
В виде html это выглядит так:
\<a href="ya.ru"\>Hi!\</a\>

Внутри ссылки не должно быть пробельных символов и переводов строк.
Внутри текста могут быть любые другие конструкции языка(кроме вложенных ссылок), не противоречащие правилам гиперссылки.

### css классы:  
В конструктор к MarkdownRenderer-у можно передать css класс, который будет добавлен ко всем тегам в результирующей странице.
В html-виде это выглядит так:
\<em class="class_name"\>Some text\</em\>

### Параграфы:
Каждая пустая строка в тексте разделяет параграфы между собой.
Несколько подряд идущих пустых строк считаются за одну.

Пример:  
Первый параграф
					
Второй параграф

Третий параграф

Параграф выделяется в html тегом <p>
То есть, пример превратится в следующий текст:
\<p\>Первый параграф\</p\>\<p\>Второй параграф\</p><p\>Третий параграф\</p\>  
Все выделения должны полностью помещаться внутрь одного параграфа.

Если в конце строки два или более пробельных символа - все эти пробельные символы заменяются на один тег \<br\>.  
Пример:  
Первая строка  
Следующая строка  
Превратится в следующее html-представление:  
Первая строка\<br\>Следующая строка

### Заголовки:
Если в начале строки стоит от 1 до 6 символов '#', это трансформируется в теги от \<h1\> до \<h6\> соответственно.  
После символов '#' должен обязательно быть по крайней мере один пробельный символ.  
Также, если после строки с заголовком идёт перенос строки - он преобразуется в тег <br>, даже если он стоит непосредственно за текстом, без пробелов.
Пример:  
# Большой заголовок(h1)
###### Маленький заголовок(h6)
Это превращается в следующий html-код:  
\<h1\>Большой заголовок\<h1\>\<br\>\<h6\>Маленький заголовок\<h6\>  

### Сode blocks
В markdown можно вставлять следующие конструкции:

    #include<iostream>
    using namespace std;
    
    int main()
    {
        cout << "Hello world!";
        return 0;
    }

В данном случае это программа "Hello world!" на языке C++.
В общем случае это может быть любой текст.  
Для того, чтобы вставить такой фрагмент, нужно в начале каждой строки написать 4 пробела или 1 символ табуляции.  
Также он должен быть выделен в один абзатц.  
В начале каждой строки этого фрагмента будет убрано по одному такому "отступу".  
Никакие правила разметки Markdown внутри такого фрагмента не действуют.  

