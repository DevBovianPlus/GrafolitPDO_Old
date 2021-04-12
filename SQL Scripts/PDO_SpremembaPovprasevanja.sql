/* Popravki Marec 2020*/
delete from NarociloPozicija_PDO;
delete from Narocilo_PDO;
delete from PovprasevanjePozicijaArtikel;
delete from PovprasevanjePozicija;
delete from Povprasevanje;
delete from KontaktnaOseba_PDO;
delete from Stranka_PDO;
delete from StrankaZaposleni_PDO;
delete from SystemEmailMessage_PDO;

dbcc checkident(NarociloPozicija_PDO, reseed,0);
dbcc checkident(Narocilo_PDO, reseed,0);
dbcc checkident(Povprasevanje, reseed,0);
dbcc checkident(PovprasevanjePozicija, reseed,0);
dbcc checkident(PovprasevanjePozicijaArtikel, reseed,0);
dbcc checkident(KontaktnaOseba_PDO, reseed,0);
dbcc checkident(Stranka_PDO, reseed,0);
dbcc checkident(StrankaZaposleni_PDO, reseed,0);
dbcc checkident(SystemEmailMessage_PDO, reseed,0);

update Nastavitve set PovprasevanjeStevilcenjeStev = 0
ALTER TABLE PovprasevanjePozicijaArtikel add DatumDobavePos datetime;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetArtikelByName](@Supplier varchar(50), @Name varchar(500))
RETURNS TABLE 
AS
RETURN 
(
SELECT I.IDENT AS StArtikla, I.NAZIV, Kategorija, Gloss, Gramatura, Velikost, Tek, DOBAVITELJ
FROM Grafolit55SI.dbo.MS MS
	JOIN DW.dbo.DIM_Identi I
		ON MS.IDENT = I.IDENT AND MS.DOBAVITELJ like '%' + @Supplier + '%'
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[GetPantheonUsers]()
RETURNS TABLE 
AS
RETURN 
(
SELECT UPPER(RTRIM(acName) + ' ' + RTRIM(acSurname)) as acSubject, acUserId, anUserID
FROM Grafolit55SI.dbo.tHE_SetSubjContact
WHERE acUserId IS NOT NULL
)


