insert into Country values(1, 'Belarus')
insert into Country values (2, 'United States Of America')
insert into Country values (3, 'Germany')
insert into Country values (4, 'Russia')

insert into Depot(DepotId, CountryId) values (1, 'FirstDepot')
insert into Depot(DepotId, DepotName) values (2, 'SomeDepot')
insert into Depot values (3, 'GermanyDepot', 3)

insert into DrugType values(1, 'FirstType')
insert into DrugType values(2, 'SecondType')


declare @id int
select @id = 1
while @id >=1 and @id <=50
begin
	insert into DrugUnit(DrugUnitId, PickNumber, DrugTypeId) values(convert(nvarchar(50),@id), convert(nvarchar(50),@id), 1)
	select @id = @id + 1
end
while @id >=51 and @id <=100
begin
	insert into DrugUnit(DrugUnitId, PickNumber, DrugTypeId) values(convert(nvarchar(50),@id), convert(nvarchar(50),@id), 2)
	select @id = @id + 1
end



