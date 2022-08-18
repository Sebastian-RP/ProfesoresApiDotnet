--mostrar todos los profesores
create proc sp_lista_instructores
as 
begin
	select
	Id, Name, DateBirth, TypeInstructor, TypeCurency Type, PriceHour
	from Instructors
end

create proc sp_guardar_instructor(
@id int,
@name varchar(40),
@dateBirth datetime,
@tipeInstructor varchar(10),
@tipeCurrency varchar(40),
@priceHour float
)as
begin
	insert into Instructors (Id, Name, DateBirth, TypeInstructor, TypeCurency, PriceHour)
	values(@id, @name, @dateBirth, @tipeInstructor, @tipeCurrency, @priceHour)
end

--mostrar todas las lecciones
create proc sp_lista_lecciones
as 
begin
	select
	Id, LessonDate, DurationLesson, InstructorId
	from Lessons
end

create proc sp_guardar_lesson2(
@lessonDate datetime,
@durationLesson int,
@instructorId int
)as
begin
	insert into Lessons(LessonDate, DurationLesson, InstructorId)
	values(@lessonDate, @durationLesson, @instructorId)
end

--llamar datos instructor con horas trabajadas

create proc sp_lista_instructor_mensualidad  
 as  
 begin  
--https://desarrolladores.me/2013/08/como-obtener-el-primer-y-ultimo-dia-del-mes-en-sql-server //sacar ultimo dia del mes actual  
 DECLARE @mydate DATETIME  
 SELECT @mydate = GETDATE()  
 select  Instructors.Id, Instructors.Name, Instructors.DateBirth,  Instructors.TypeInstructor, Instructors.TypeCurency, Instructors.PriceHour,  
 --el campo Durationlesson el que sea nul lo vuelve 0 y sumara todas las clases y mostrara el resultado en horas, del mismo mes   
 ISNULL(round(convert(FLOAT, Sum(Lessons.DurationLesson))/60, 1), 0) as DurationLesson  
 from Instructors  
 FULL JOIN Lessons   
 on Instructors.Id = Lessons.InstructorId  
 where (Lessons.DurationLesson is null) or Lessons.LessonDate between EOMONTH ( @mydate, -1 ) and EOMONTH( @mydate )  
 group by Instructors.Id, Instructors.Name, Instructors.DateBirth,  Instructors.TypeInstructor, Instructors.TypeCurency, Instructors.PriceHour  
 order by Instructors.Name  
end

--manera de obtener el codigo de un procedimiento almacenado
--sp_helptext nombre_sp