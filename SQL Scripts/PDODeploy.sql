
CREATE FUNCTION [dbo].[GetArtikelByNamePantheonOnly](@Supplier varchar(50), @Name varchar(500))
RETURNS TABLE 
AS
RETURN 
(
SELECT MS.IDENT AS StArtikla, MS.NAZIV, MS.DOBAVITELJ
FROM Grafolit55SI.dbo.MS MS
WHERE MS.DOBAVITELJ like '%' + @Supplier + '%' AND MS.NAZIV LIKE '%' + @Name + '%'
-- PREMAZ MATT
and (MS.NAZIV LIKE '%' + REPLACE(@Name, 'MATT', 'SILK') + '%'
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'MATT', 'SATIN') + '%'
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'MATT', 'MAT') + '%'
-- PREMAZ GLOSS
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'GLOSS', 'GLOS') + '%'
-- OFFSET
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET AMBER') + '%'
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET PRINT') + '%'
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'ARCOSET EXTRA WHITE') + '%'
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'ROYALSET EXTRA WHITE') + '%'
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET PREPRINT') + '%'
OR MS.NAZIV LIKE '%' + REPLACE(@Name, 'OFFSET', 'OFFSET SELENA') + '%')
)


alter table PovprasevanjePozicijaArtikel add DobaviteljID int;
alter table PovprasevanjePozicijaArtikel add DobaviteljNaziv_PA varchar(1000);
alter table KontaktnaOseba_PDO add IsNabava bit;

ALTER TABLE PovprasevanjePozicijaArtikel  WITH CHECK ADD  CONSTRAINT FK_DobaviteljPozivijaArtikelID FOREIGN KEY(DobaviteljID)
REFERENCES Stranka_PDO (StrankaID)
GO
ALTER TABLE PovprasevanjePozicijaArtikel CHECK CONSTRAINT FK_DobaviteljPozivijaArtikelID
GO







