create database Online_Assessment
use Online_Assessment
LAPTOP-2IMRD289

create table Admin_table(
Admin_Id int primary key identity(1,1),
Admin_name varchar(100) not null unique,
Password varchar(50) not null
)

create table User_table(
User_Id int primary key identity(1,1),
First_name varchar(100) not null,
Last_name varchar(100) not null,
Email varchar(100) not null unique,
Password varchar(50) not null
)

create table Subject_table(
Subject_Id int primary key identity(0,1),
Subject_name varchar(100) not null unique
)

create table Difficulty_table(
Difficulty_Id int primary key identity(0,1),
Difficulty_level varchar(50) not null unique
)

create table Question_table(
Question_Id int primary key identity(1,1),
Subject_id int foreign key references subject_table(subject_id) not null,
Questions varchar(255) not null,
Difficulty_Id int foreign key references Difficulty_table(Difficulty_Id) not null
)

create table Option_table(
Option_Id int primary key identity(1,1),
Question_Id int foreign key references Question_table(Question_Id) not null,
Options varchar(255) not null,
Answer bit not null
)

create table Test_table(
Test_Id int primary key identity(1,1),
Test_name varchar(255) not null unique,
Created_date datetime not null,
Start_date datetime not null,
End_date datetime not null,
Duration time(0)
)

create table Question_mapping_table(
Question_map_id int primary key identity(1,1),
Test_Id int foreign key references Test_table(Test_Id) not null,
Question_Id int foreign key references Question_table(Question_Id) not null,
)

create table Test_invitation_table(
Invitation_Id int primary key identity(1,1),
Test_Id int foreign key references Test_table(Test_Id) not null,
User_email varchar(100) foreign key references user_table(email),
Invited_date datetime not null
)

create table Answer_table(
Answer_Id int primary key identity(1,1),
Test_Id int foreign key references Test_table(Test_Id) not null,
User_Id int foreign key references User_table(User_Id) not null,
Question_Id int foreign key references Question_table(Question_Id) not null,
Option_Id int foreign key references option_table(Option_id) not null,
Submit_date datetime not null
)

select*from test_table

insert into subject_table
values('Select'),
('HTML'),
('Asp.Net'),
('SQL'),
('All')
insert into Difficulty_table
values('Select'),
('Entry-level'),
('Mid-level'),
('Senior-level'),
('All')

create procedure Get_questions
@Subject_id int = 4,@Difficulty_id int = 4
as begin
if @Subject_id<4 and @Difficulty_id<4
begin
select question_id,Questions,st.Subject_name,Difficulty_level
from Question_table qt
inner join Subject_table st on st.Subject_Id = qt.Subject_Id
inner join Difficulty_table
on qt.Difficulty_Id=Difficulty_table.Difficulty_Id
where st.Subject_Id=@Subject_id and qt.Difficulty_Id=@Difficulty_id
end
else
begin
select Question_Id,Questions,st.Subject_name,Difficulty_level
from Question_table qt
inner join Subject_table st on st.Subject_Id = qt.Subject_Id
inner join Difficulty_table
on qt.Difficulty_Id=Difficulty_table.Difficulty_Id
end
end

select Question_Id 
from Question_mapping_table
where Test_Id=1

delete Question_mapping_table
where Test_Id=1

select*from Question_mapping_table

create procedure Find_exist_test
@test_id int
as begin
declare @result int
if exists (select 1 from Question_mapping_table where Test_Id=1)
begin
set @result = 1
end
else
begin
set @result = 0
end
select @result as Result
end

select question_id,Questions,st.Subject_name,Difficulty_level
from Question_table qt
inner join Subject_table st on st.Subject_Id = qt.Subject_Id
inner join Difficulty_table
on qt.Difficulty_Id=Difficulty_table.Difficulty_Id
where st.Subject_Id=2 and qt.Difficulty_Id=2

