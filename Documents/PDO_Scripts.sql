use GrafolitOTP_Prod2
SELECT 
    c.name 'Column Name',
    t.Name 'Data type',
    c.max_length 'Max Length',
    c.precision ,
    c.scale ,
    c.is_nullable,
    ISNULL(i.is_primary_key, 0) 'Primary Key'
FROM    
    sys.columns c
INNER JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN 
    sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN 
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
    c.object_id = OBJECT_ID('SystemMessageEvents_OTP')


create table TipStranka_PDO(
	TipStrankaID int not null identity(1,1) primary key,
	Koda varchar(20) not null,
	Naziv varchar(50) not null,
	Opis varchar(250) null,
	tsIDOseba int null,
	ts datetime null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
);

create table Stranka_PDO(
	StrankaID int not null identity(1,1) primary key,
	KodaStranke varchar(250) null,
	TipStrankaID int not null,
	NazivPrvi varchar(250) null,
	NazivDrugi varchar(300) null,
	Naslov varchar(250) null,
	StevPoste varchar(12) null,
	NazivPoste varchar(250) null,
	Email varchar(50) null,
	Telefon	varchar(50) null,
	FAX	varchar(50) null,
	InternetniNalov	varchar(50) null,
	KontaktnaOseba varchar(50) null,
	RokPlacila nchar(6) null,
	TRR	varchar(35) null,
	DavcnaStev varchar(20) null,
	MaticnaStev	varchar(20) null,
	StatusDomacTuji	bit null,
	Zavezanec_DA_NE	bit null,
	IdentifikacijskaStev varchar(20) null,
	Clan_EU	bit null,
	BIC	varchar(11) null,
	KodaPlacila	varchar(4) null,
	DrzavaStranke varchar(50) null,
	Neaktivna varchar(2) null,
	GenerirajERacun	int null,
	JavniZavod int null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_TipStrankeID foreign key(TipStrankaID) references TipStranka_PDO(TipStrankaID)
);

create table Vloga_PDO(
	VlogaID	int not null identity(1,1) primary key,
	Koda varchar(60) null,
	Naziv varchar(160) null,
	ts	datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
);

create table Osebe_PDO(
	OsebaID	int not null identity(1,1) primary key,
	VlogaID	int not null,
	Ime	varchar(60) not null,
	Priimek	varchar(60) null,
	Naslov varchar(160) null,
	DatumRojstva date null,
	Email varchar(200) null,
	DatumZaposlitve	date null,
	UporabniskoIme varchar(100) null,
	Geslo varchar(100) null,
	TelefonGSM varchar(60) null,
	Zunanji	int null,
	DelovnoMesto varchar(200) null,
	ProfileImage varchar(300) null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_VlogaID foreign key(VlogaID) references Vloga_PDO(VlogaID)
);

create table OsebeNadrejeni_PDO(
	OsebeNadrejeniID int not null identity(1,1) primary key,
	OsebaID	int not null,
	NadrejeniID int not null,
	Opomba varchar(200) null,
	ts datetime null,
	tsIDosebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_OsebaID foreign key(OsebaID) references Osebe_PDO(OsebaID),
	constraint FK_NadrejeniID foreign key(NadrejeniID) references Osebe_PDO(OsebaID)
);

create table StrankaZaposleni_PDO(
	StrankaOsebeID int not null identity(1,1) primary key,
	StrankaID int not null,
	OsebeID	int not null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_StrankaID foreign key(StrankaID) references Stranka_PDO(StrankaID),
	constraint FK_OsebeID foreign key(OsebeID) references Osebe_PDO(OsebaID)
);

create table KontaktnaOseba_PDO(
	KontaktnaOsebaID int not null identity(1,1) primary key,
	StrankaID int null,
	NazivKontaktneOsebe	varchar(50) not null,
	Telefon	varchar(50) null,
	GSM	varchar(50) null,
	Email varchar(50) null,
	DelovnoMesto varchar(50) null,
	ZaporednaStevika int null,
	Fax	varchar(30) null,
	Opombe varchar(1000) null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_K_StrankaID foreign key(StrankaID) references Stranka_PDO(StrankaID)
);

create table StatusPovprasevanja(
	StatusPovprasevanjaID int not null identity(1,1) primary key,
	Koda varchar(20) null,
	Naziv varchar(50) null,
	Opis varchar(250) null,
	tsIDOseba int null,
	ts	datetime null,
	tsUpdate datetime null,
	tsUpdateUserID int null
);

create table SystemEmailMessage_PDO(
	SystemEmailMessageID int not null identity(1,1) primary key,
	EmailFrom varchar(100) null,
	EmailTo	varchar(100) null,
	EmailBody varchar(MAX) null,
	EmailSubject varchar(200) null,
	Status	int null,
	ts	datetime null,
	tsIDOsebe	int null,
	tsUpdate datetime null,
	tsUpdateUserID int null
);

create table SystemMessageEvents_PDO(
	SystemMessageEventsID int not null identity(1,1) primary key,
	MasterID int not null,
	Code varchar(50) not null,
	Status int null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null
);

create table Povprasevanje(
	PovprasevanjeID int not null identity(1,1) primary key,
	KupecID int not null,
	StatusID int not null,
	Naziv varchar(300) not null,
	PovprasevanjeStevilka varchar(30) not null,
	NarociloID int null,
	DatumOddajePovprasevanja datetime null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_KupecID foreign key(KupecID) references Stranka_PDO(StrankaID),
	constraint FK_StatusID foreign key(StatusID) references StatusPovprasevanja(StatusPovprasevanjaID)
);

create table PovprasevanjePozicija(
	PovprasevanjePozicijaID int not null identity(1,1) primary key,
	PovprasevanjeID int not null,
	KategorijaNaziv varchar(200) not null,
	KategorijaID varchar(50) not null,
	Naziv varchar(200) not null,
	Kolicina1 decimal(18,3) null,
	EnotaMere1 varchar(30) null,
	Kolicina2 decimal(18,3) null,
	EnotaMere2 varchar(30) null,
	Opombe varchar(3000) null,
	DatumPredvideneDobave datetime null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_PovprasevanjeID foreign key(PovprasevanjeID) references Povprasevanje(PovprasevanjeID)
);

create table PovprasevanjePozicijaDobavitelj(
	PovprasevanjePozicijaDobaviteljID int not null identity(1,1) primary key,
	PovprasevanjePozicijaID int not null,
	DobaviteljID int null,
	DobaviteljNaziv_P varchar(300) null,
	DobaviteljID_P varchar(50) null,
	KupecViden bit null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_PovprasevanjePozicijaID foreign key(PovprasevanjePozicijaID) references PovprasevanjePozicija(PovprasevanjePozicijaID),
	constraint FK_DobaviteljID foreign key(DobaviteljID) references Stranka_PDO(StrankaID)
);

/*----- NarociloStevilka_P bomo pridobili iz pantheona ko se bo narocilnica generirala oz. ko bomo oddali naroèilo dobavitelju -----*/

create table Narocilo(
	NarociloID int not null identity(1,1) primary key,
	NarociloStevilka_P varchar(50) null,
	PovprasevanjePozicijaID int not null,
	IzbranDobaviteljID int not null,
	IzbraniArtikelNaziv varchar(200) not null,
	IzbraniArtikelID_P varchar(50) not null,
	ArtikelCena decimal(18,3) not null,
	DatumDobave datetime null,
	Opombe varchar(1000) null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_N_PovprasevanjePozicijaID foreign key(PovprasevanjePozicijaID) references PovprasevanjePozicija(PovprasevanjePozicijaID),
	constraint FK_IzbranDobaviteljID foreign key(IzbranDobaviteljID) references Stranka_PDO(StrankaID),
);


create table Nastavitve(
	NastavitveID int not null identity(1,1) primary key,
	PovprasevanjeStevilcenjeStev int null,
	PovprasevanjeStevilcenjePredpona varchar(10) null,
	Opombe varchar(1000) null,
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
);

alter table Povprasevanje
add Zakleni bit null,
ZaklenilUporabnik int null;

alter table Povprasevanje
add DatumPredvideneDobave datetime null;

alter table Nastavitve
add PosiljanjePoste bit null

alter table Osebe_PDO
add PDODostop bit null

alter table PovprasevanjePozicijaDobavitelj
add PotDokumenta varchar(1000) null

alter table Osebe_PDO
add EmailGeslo varchar(50) null,
EmailStreznik varchar(50) null,
EmailVrata int null,
EmailSifriranjeSSL bit null;

alter table Nastavitve
add EmailStreznik varchar(50) null,
EmailVrata int null,
EmailSifriranjeSSL bit null;

alter table SystemEmailMessage_PDO
add OsebaEmailFromID int null,
constraint FK_SysEmailMsg_OsebaID foreign key(OsebaEmailFromID) references Osebe_PDO(OsebaID);

alter table Osebe_PDO
add Podpis varchar(100) null;

alter table SystemEmailMessage_PDO
add Attachments varchar(4000) null;

alter table Povprasevanje
add PovprasevajneOddal int null,
constraint FK_Employee_OsebaID foreign key(PovprasevajneOddal) references Osebe_PDO(OsebaID);


insert into StatusPovprasevanja
values ('ODDANO','Povpraševanje oddano',NULL,NULL, GetDate(),GetDate(), NULL),
('NAROCENO','Naroèilnica kreirana',NULL,NULL, GetDate(),GetDate(), NULL);


select * from StatusPovprasevanja

select * from Povprasevanje order by 1 desc

select * from Stranka_PDO order by 1 desc

select * from TipStranka_PDO

select * from GetSuplierBySearchStr('ARTA TRADE SRL')

select * from PovprasevanjePozicijaDobavitelj

select * from GetArtikelBySearchStr('%SELO%')

select * from Narocilo_PDO
select * from NarociloPozicija_PDO

select * from PovprasevanjePozicijaDobavitelj order by 1 desc

select * from SystemEmailMessage_PDO order by 1 desc

delete from SystemEmailMessage_PDO
dbcc checkident(SystemEmailMessage_PDO, reseed,0)

select * from TipStranka_PDO

select * from Vloga_PDO

select * from Osebe_PDO

select * from PovprasevanjePozicija order by 1 desc

select * from SystemEmailMessage_PDO order by 1 desc

select * from Povprasevanje where PovprasevajneOddal is not null

delete from Povprasevanje
dbcc checkident(Povprasevanje, reseed,0)

delete from Narocilo_PDO
dbcc checkident(Narocilo_PDO, reseed,0)


/*Martin => 7.2.2020 */
create table KontaktnaOseba_PDO(
	KontaktneOsebeID int not null identity(1,1) primary key,
	StrankaID int not null,
	Naziv varchar(300) not null,
	NazivPodpis	varchar(300) null,
	Telefon varchar(50) null,
	GSM varchar(50) null,
	Email varchar(50) null,
	Fax varchar(30) null,
	Opombe varchar(1000) null,	
	ts datetime null,
	tsIDOsebe int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,

	constraint FK_StrankaID foreign key(StrankaID) references Stranka_PDO(StrankaID)
);


/* Popravki Marec 2020*/

delete from Povprasevanje;
delete from NarociloPozicija_PDO;
delete from Narocilo_PDO;
delete from SystemEmailMessage_PDO;

dbcc checkident(Povprasevanje, reseed,0);
dbcc checkident(NarociloPozicija_PDO, reseed,0);
dbcc checkident(Narocilo_PDO, reseed,0);

IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'FK_N_PovprasevanjePozicijaID' AND type = N'F')
ALTER TABLE NarociloPozicija_PDO DROP CONSTRAINT FK_N_PovprasevanjePozicijaID;
GO

IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'PovprasevanjePozicijaArtikel') AND type in (N'U'))
DROP TABLE PovprasevanjePozicijaArtikel
GO

IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'PovprasevanjePozicijaDobavitelj') AND type in (N'U'))
DROP TABLE PovprasevanjePozicijaDobavitelj
GO

IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'PovprasevanjePozicija') AND type in (N'U'))
	DROP TABLE PovprasevanjePozicija
GO

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


alter table Stranka_PDO add PrivzetaEM varchar (50);
alter table Stranka_PDO add ZadnjaIzbranaKategorija varchar (100);

update StatusPovprasevanja set Koda = 'PRIPRAVLJENO', Naziv = 'Povpraševanje pripravljeno', Opis = 'Povpraševanje pripravljeno' where Koda = 'ERR_ORDER_NO_SEND'

delete from Nastavitve;
dbcc checkident(Povprasevanje, reseed,0);

INSERT INTO Nastavitve(PovprasevanjeStevilcenjeStev,PovprasevanjeStevilcenjePredpona,ts,tsIDOsebe) VALUES(0, 2020, '03-01-2020', 3)
GO

exec sp_rename 'KontaktnaOseba_PDO.NazivKontaktneOsebe', 'Naziv', 'COLUMN';

-- 30.03.2020 - Task 133 - Pri imenu dobavitelja naj sistem upošteva koren besede, da nanj vleèe vse obstojeèe šifre iz sistema (primer: vleèe vse po kljuèni besedi SAPPI, ne pa izkljuèno pod SAPPI EU, SAPPI DE, SAPPI HOLDING…)
USE [GrafolitPDO]
GO
/****** Object:  UserDefinedFunction [dbo].[GetArtikelByName]    Script Date: 27. 03. 2020 14:54:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetArtikelByName](@Supplier varchar(50), @Name varchar(500))
RETURNS TABLE 
AS
RETURN 
(
SELECT MS.DOBAVITELJ, I.IDENT AS StArtikla, I.NAZIV, Kategorija, Gloss, Gramatura, Velikost, Tek
FROM Grafolit55SI.dbo.MS MS
	JOIN DW.dbo.DIM_Identi I
		ON MS.IDENT = I.IDENT AND MS.DOBAVITELJ like '' + @Supplier + '%'
WHERE	I.NAZIV LIKE '%' + @Name + '%'
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




