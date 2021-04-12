alter table PovprasevanjePozicijaArtikel add Poreklo varchar(10);
alter table NarociloPozicija_PDO add Poreklo varchar(10);

USE [GrafolitPDO]
GO
/****** Object:  UserDefinedFunction [dbo].[GetArtikelByName]    Script Date: 5. 03. 2021 07:44:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetArtikelByName](@Supplier varchar(50), @Name varchar(500))
RETURNS TABLE 
AS
RETURN 
(
SELECT I.IDENT AS StArtikla, I.NAZIV, Kategorija, Gloss, Gramatura, Velikost, Tek, DOBAVITELJ, MS.POREKLO
FROM Grafolit55SI.dbo.MS MS
JOIN DW.dbo.DIM_Identi I
ON MS.IDENT = I.IDENT AND MS.DOBAVITELJ like '%' + @Supplier + '%'
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
