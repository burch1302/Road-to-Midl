DECLARE @id nvarchar(50);
--курсоры - очень медленные но покрывает некоторый стек задач

--курсор позвол€ет итеративно проходить по каждой строке запроса


declare my_cursor cursor for 
select top 5 * from Contact

open my_cursor
fetch nex from my_cursor into @id

while @@FETCH_STATUS = 0
begin 
	
	fetch next from my_cursor;
end

close my_cursor;
deallocate my_cursor;

-- ѕростой курсор
DECLARE @Name nvarchar(50);

declare contactName_cursor Cursor for
Select top 5 Name from Contact;

open contactName_cursor
fetch next from contactName_cursor into @Name;
while @@FETCH_STATUS = 0

begin
	print @Name
	fetch NEXT from contactName_cursor into @Name
end

close contactName_cursor
deallocate contactName_cursor;

--“ипы курсора

--как можно оптимизировать курсор?
	-- использовать FAST_FORWARD - нужен только дл€ чтени€ вперед, но он самый быстрый
	-- использовать LOCAL STATIC READ_ONLY FORWARD_ONLY
		--LOCAL делает курсор локальным , например в хранимке.  ак только она закончит работу курсор дропнетс€
		--STATIC делает снапшот запроса на котор≥й открыт курсор, то есть если кто то будет мен€ть в это врем€ данные, это не повли€ет
		--READ_ONLY делает курсор только дл€ чтени€ 
		--FORWARD_ONLY курсор бежит только вперед , и его нельз€ отматать назад SQL не хранит позиции дл€ обратного чтени€

	--Ќе выполн€ть сложные операции внутри цикла, апдейт вообще € хз когда можно юзать в курсоре , гуру пишут что никогда
	-- урсор построчна€ обработка , по этому логично что на больших объемах данных он офигеет

--≈ще раз. “ипы:
 --FAST_FORWARD
 --KEYSET - компромис между статическим и динамическим (он запоминает только ключ)
 --STATIC

 --вот какие есть команды дл€ курсора
 FETCH FIRST FROM cursor_scroll INTO @Name;

 FETCH NEXT FROM cursor_scroll INTO @Name;

 FETCH PRIOR FROM cursor_scroll INTO @Name; -- это возврат на пред идущую

 FETCH ABSOLUTE 5 FROM cursor_scroll INTO @Name; --5 строка по номеру 

 -- огда использовать курсор ?  огда например дл€ каждой записи , 
 --нужно запустить уникальную процедуру в зависимости от ее значений

--вот сейчас € курсором апну базовую валюту счетов на 100 гревень

declare @amount decimal
declare @invoiceId nvarchar(50)

declare uppAmountCursor Cursor for
select top 5 PrimaryAmount, id from Invoice
where PrimaryAmount != 0
order by CreatedOn desc

open uppAmountCursor
	fetch next from uppAmountCursor into @amount, @invoiceId
	while @@FETCH_STATUS = 0

	begin
		update Invoice
		set PrimaryAmount = @amount + 100
		where id = @invoiceId
		fetch next from uppAmountCursor into @amount, @invoiceId -- фу фу никогда так не делать (делаю только дл€ навыка)
	end

close uppAmountCursor
deallocate uppAmountCursor
