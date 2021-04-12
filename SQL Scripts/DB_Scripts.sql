-- 21.12.2019 - PDO - Polja za pantheon klice in preverjanja
alter table Narocilo_PDO add P_CreateOrder datetime, P_UnsuccCountCreatePDFPantheon int, P_LastTSCreatePDFPantheon datetime, P_TransportOrderPDFName varchar(50), P_TransportOrderPDFDocPath varchar(400), P_GetPDFOrderFile datetime
INSERT INTO StatusPovprasevanja values('USTVARJENO_NAROCILO', 'Ustvarjeno naroèilo v Pantheon','Naroèilo je bilo uspešno ustvarjeno v Pantheon-u', 1,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1); 
INSERT INTO StatusPovprasevanja values('KREIRAN_PDF', 'Pantheon je kreiral PDF','Pantheon je kreiral PDF,ki se ga pošlje prevozniku', 1,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1); 

-- 14.01.2020 - PDO - dodajanje Jezika
create table Jeziki (
	JezikID int not null identity(1,1) primary key,
	Koda varchar(50) not null,
	Naziv varchar(500),
	ts datetime null,	
);

alter table Stranka_PDO add JezikID int null,
constraint FK_Jezik foreign key(JezikID) references Jeziki(JezikID);

Insert into Jeziki values('ANG', 'Anglešèina', '01.01.2020');
Insert into Jeziki values('SLO', 'Slovenšèina', '01.01.2020');
Insert into Jeziki values('HRV', 'Hrvašèina', '01.01.2020');



-- 14.01.2020 - Dodajanje kolièine v osnovni merski enoti 
alter table NarociloPozicija_PDO add KolicinavKG decimal(18,2);
alter table NarociloPozicija_PDO add EnotaMere varchar(10);
alter table NarociloPozicija_PDO add Rabat decimal(18,2);
alter table NarociloPozicija_PDO add OpombaNarocilnica varchar(5000);

insert into StatusPovprasevanja(Koda, Naziv, Opis, tsIDOseba, ts) values('ERR_ORDER_NO_SEND', 'Naroèilnica NI bila poslana', 'Zaradi napake naroèilnica ni bila poslana', 1, '1.1.2020');
insert into StatusPovprasevanja(Koda, Naziv, Opis, tsIDOseba, ts) values('ERR_ADMIN_MAIL', 'Obvestilo admin, NI poslana naroèilnica', 'Obvestilo admin, NI poslana naroèilnica', 1, '1.1.2020');


-- 16.01.2020 - PDO - dodajanje oddelek
create table Oddelek (
	OddelekID int not null identity(1,1) primary key,
	Koda varchar(50) not null,
	Naziv varchar(500),
	ts datetime null,	
);

alter table Narocilo_PDO add OddelekID int null,
constraint FK_Oddelek foreign key(OddelekID) references Oddelek(OddelekID);

alter table NarociloPozicija_PDO add OddelekID int null,
constraint FK_OddelekNArociloPozicija foreign key(OddelekID) references Oddelek(OddelekID);

Insert into Oddelek values('15', '15 - SKLADIŠÈE MALOPRODAJA', '01.01.2020');
Insert into Oddelek values('16', '16 - TRANZIT', '01.01.2020');
Insert into Oddelek values('17', '17 - MALOPRODAJA ZA KUPCE', '01.01.2020');
Insert into Oddelek values('18', '18 - MP ZA KUPCE - PREKLAD', '01.01.2020');

alter table NarociloPozicija_PDO add PrikaziKupca bit null;

alter table PovprasevanjePozicija add Dobavitelji varchar(5000);

update StatusPovprasevanja set Koda = 'KREIRAN_POSLAN_PDF', Naziv = 'Kreiran PDF in naroèilnica je poslana dobavitelju', Opis = 'Pantheon je kreiral PDF,ki se ga pošlje prevozniku' where Koda = 'KREIRAN_PDF'

--21.02.2020, Spremembe
drop table PovprasevanjePozicija;
drop table PovprasevanjePozicijaArtikel;

create table PovprasevanjePozicija(
	PovprasevanjePozicijaID int not null identity(1,1) primary key,
	PovprasevanjeID int not null,
	DobaviteljID int not null,
	DobaviteljNaziv_P varchar(300) null,
	DobaviteljKontaktOsebe varchar(300) null,
	DobaviteljID_P varchar(50) null,
	Artikli varchar(5000) null,
	KupecViden bit null,
	DatumPredvideneDobave datetime null,
	PotDokumenta varchar(3000) null,
	Opomba varchar(3000) null,
	EmailBody varchar(3000) null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
	constraint FK_DobaviteljID foreign key(DobaviteljID) references Stranka_PDO(StrankaID),	
	constraint FK_PovprasevanjeID foreign key(PovprasevanjeID) references Povprasevanje(PovprasevanjeID)
);

create table PovprasevanjePozicijaArtikel(
	PovprasevanjePozicijaArtikelID int not null identity(1,1) primary key,
	PovprasevanjePozicijaID int not null,
	KategorijaNaziv varchar(200) not null,	
	Naziv varchar(200) not null,
	Kolicina1 decimal(18,3) null,
	EnotaMere1 varchar(30) null,
	Kolicina2 decimal(18,3) null,
	EnotaMere2 varchar(30) null,
	Opombe varchar(3000) null,	
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_PovprasevanjePozicijaID foreign key(PovprasevanjePozicijaID) references PovprasevanjePozicija(PovprasevanjePozicijaID),	
);

alter table Povprasevanje add NotSendPDFAndEmailsToSupplier bit;
alter table NarociloPozicija_PDO add IzbranArtikel bit;
insert into StatusPovprasevanja(Koda, Naziv, Opis, tsIDOseba, ts) values('PREVERJENI_ARTIKLI', 'Preverjeni artikli - Pantheon', 'Preverjeni artikli - Pantheon', 1, '1.1.2020');
update StatusPovprasevanja set Koda = 'POSLANO_V_NABAVO', Naziv = 'Naroèilo poslano v nabavo', Opis = 'Naroèilo poslano v nabavo' where Koda = 'KREIRAN_POSLAN_PDF';

Create FUNCTION [dbo].[GetArtikelByName](@Supplier varchar(50), @Name varchar(500))
RETURNS TABLE 
AS
RETURN 
(
SELECT I.IDENT AS StArtikla, I.NAZIV, Kategorija, Gloss, Gramatura, Velikost, Tek
FROM Grafolit55SI.dbo.MS MS
JOIN DW.dbo.DIM_Identi I
ON MS.IDENT = I.IDENT AND MS.DOBAVITELJ = @Supplier
WHERE I.NAZIV LIKE '%' + @Name + '%'
-- PREMAZ MATT
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'MATT', 'SILK') + '%'
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'MATT', 'SATIN') + '%'
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'MATT', 'MAT') + '%'
-- PREMAZ GLOSS
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'GLOSS', 'GLOS') + '%'
-- OFFSET
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET AMBER') + '%'
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET PRINT') + '%'
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'ARCOSET EXTRA WHITE') + '%'
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'ROYALSET EXTRA WHITE') + '%'
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET PREPRINT') + '%'
OR I.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET SELENA') + '%'
)

alter table PovprasevanjePozicija add IzbranArtikel bit;
alter table Povprasevanje add Dobavitelji varchar(400);

/* Popravki Marec 2020*/
delete from Povprasevanje;
delete from NarociloPozicija_PDO;
delete from Narocilo_PDO;

dbcc checkident(Povprasevanje, reseed,0);
dbcc checkident(NarociloPozicija_PDO, reseed,0);
dbcc checkident(Narocilo_PDO, reseed,0);


alter table NarociloPozicija_PDO DROP CONSTRAINT FK_N_PovprasevanjePozicijaID;
drop table PovprasevanjePozicija;
drop table PovprasevanjePozicijaDobavitelj;
drop table PovprasevanjePozicijaArtikel;

-- tole kasneje

create table PovprasevanjePozicija(
	PovprasevanjePozicijaID int not null identity(1,1) primary key,
	PovprasevanjeID int not null,
	DobaviteljID int not null,
	DobaviteljNaziv_P varchar(300) null,
	DobaviteljKontaktOsebe varchar(300) null,
	DobaviteljID_P varchar(50) null,
	Artikli varchar(5000) null,
	KupecViden bit null,
	DatumPredvideneDobave datetime null,
	PotDokumenta varchar(3000) null,
	Opomba varchar(3000) null,
	EmailBody varchar(3000) null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
	constraint FK_DobaviteljID foreign key(DobaviteljID) references Stranka_PDO(StrankaID),	
	constraint FK_PovprasevanjeID foreign key(PovprasevanjeID) references Povprasevanje(PovprasevanjeID)
);

create table PovprasevanjePozicijaArtikel(
	PovprasevanjePozicijaArtikelID int not null identity(1,1) primary key,
	PovprasevanjePozicijaID int not null,
	KategorijaNaziv varchar(200) not null,	
	Naziv varchar(200) not null,
	Kolicina1 decimal(18,3) null,
	EnotaMere1 varchar(30) null,
	Kolicina2 decimal(18,3) null,
	EnotaMere2 varchar(30) null,
	Opombe varchar(3000) null,	
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
	Ident varchar(50) NULL,
	IzbraniArtikelNaziv_P varchar(200) NULL,
	ArtikelCena decimal(18, 3) NULL,	
	KolicinavKG decimal(18, 2) NULL,
	EnotaMere varchar(10) NULL,
	Rabat decimal(18, 2) NULL,
	OpombaNarocilnica varchar(5000) NULL,
	OddelekID int NULL,
	PrikaziKupca bit NULL,
	IzbranArtikel bit NULL,

	constraint FK_PovprasevanjePozicijaArtikelID foreign key(PovprasevanjePozicijaID) references PovprasevanjePozicija(PovprasevanjePozicijaID),	
	constraint FK_OddelekID foreign key(OddelekID) references Oddelek(OddelekID),	
	
);

alter table Povprasevanje add Dobavitelji varchar (500);
alter table Povprasevanje add OpombaNarocilnica varchar (5000);
alter table Povprasevanje add NotSendPDFAndEmailsToSupplier bit;

select * from StatusPovprasevanja
alter table Povprasevanje add Narocila varchar (500);
insert into StatusPovprasevanja(Koda, Naziv, Opis, tsIDOseba, ts) values('PREVERJENI_ARTIKLI', 'Preverjeni artikli - Pantheon', 'Preverjeni artikli - Pantheon', 1, '1.1.2020');
update StatusPovprasevanja set Koda = 'POSLANO_V_NABAVO', Naziv = 'Naroèilo poslano v nabavo', Opis = 'Naroèilo poslano v nabavo' where StatusPovprasevanjaID = 9

alter table Povprasevanje add OddelekID int null;
ALTER TABLE Povprasevanje WITH CHECK ADD  CONSTRAINT FK_PovprasevanjeOddelekID FOREIGN KEY(OddelekID) REFERENCES Oddelek(OddelekID);
ALTER TABLE NarociloPozicija_PDO WITH CHECK ADD  CONSTRAINT FK_N_PovprasevanjePozicijaID FOREIGN KEY(PovprasevanjePozicijaID) REFERENCES PovprasevanjePozicija(PovprasevanjePozicijaID);

alter table Povprasevanje drop constraint FK_Povprasevanje_Narocilo_PDO;

delete from Narocilo_PDO
alter table Narocilo_PDO add PovprasevanjeID int not null;
alter table Narocilo_PDO add PovprasevanjeStevilka varchar (50);

alter table KontaktnaOseba_PDO add NazivPodpis varchar (300);



Alter table Narocilo_PDO drop column Jamada1;
Alter table Narocilo_PDO drop column Jamada2;
Alter table Narocilo_PDO drop column Jamada3;
Alter table Narocilo_PDO drop column Jamada4;
Alter table Narocilo_PDO drop column Jamada5;
Alter table Narocilo_PDO drop column Jamada6;

sp_rename 'KontaktnaOseba_PDO.NazivKontaktneOsebe', 'Naziv', 'COLUMN';