select * from OdpoklicPozicija order by 1 desc

update Povprasevanje set NotSendPDFAndEmailsToSupplier = 0;

select * from StatusPovprasevanja
select * from Povprasevanje where StatusID = 9 order by ts desc
select * from PovprasevanjePozicija where PovprasevanjePozicijaID = 
select * from PovprasevanjePozicijaArtikel where PovprasevanjePozicijaID = 464
select * from PovprasevanjePozicijaArtikel  order by 1 desc
select * from PovprasevanjePozicija where PovprasevanjeID = 444 order by 1 desc
select * from PovprasevanjePozicija order by ts desc
select * from PovprasevanjePozicija order by 1 desc
select * from PovprasevanjePozicijaArtikel  order by 1 desc
select * from Povprasevanje order by 1 desc

select * from NarociloPozicija_PDO  order by 1 desc
select * from Stranka order by 1 desc
	
select * from Osebe_PDO
select * from Narocilo_PDO order by 1 desc
select * from NarociloPozicija_PDO order by 1 desc

select pp.PovprasevanjePozicijaID, pp.DobaviteljNaziv_P, ppa.KategorijaNaziv, ppa.Naziv, ppa.Kolicina1, ppa.EnotaMere1, ppa.IzbranArtikel 
from PovprasevanjePozicijaArtikel ppa  inner join PovprasevanjePozicija pp on pp.PovprasevanjePozicijaID = ppa.PovprasevanjePozicijaID 
where pp.PovprasevanjeID = 1043

alter table PovprasevanjePozicija add ObvesceneOsebe varchar(400);
--alter table Povprasevanje add Dobavitelji varchar(400);

delete from Narocilo_PDO
delete from NarociloPozicija_PDO

select * from Stranka_PDO where StrankaID = 24 order by 1 desc
select * from Stranka_PDO order by 1 desc
select * from SystemEmailMessage_PDO where Status = 0 order by 1 desc
select * from SystemEmailMessage_PDO order by 1 desc

select * from GetArtikelByName('ARCTI', '64X90')
select * from GetArtikelByNamePantheonOnly('R.D.M.', '150G 45X85 SB')
SELECT MS.IDENT, MS.DOBAVITELJ, MS.* FROM Grafolit55SI.dbo.MS MS  where MS.DOBAVITELJ like '%R.D.M. OVARO%' and MS.IDENT = '0712080301047   '
SELECT MS.IDENT, MS.DOBAVITELJ, MS.* FROM Grafolit55SI.dbo.MS MS  where MS.DOBAVITELJ like '%R.D.M. OVARO%' and MS.IDENT = '%130G 45X64 SB%'
SELECT MS.IDENT, MS.DOBAVITELJ, MS.* FROM Grafolit55SI.dbo.MS MS  where MS.NAZIV like '%130G 45X64 SB%'
select * from DW.dbo.DIM_Identi where IDENT = '0712080301047   '
select * from DW.dbo.DIM_Identi where NAZIV like '%130G 45X64 SB%'
SELECT MS.IDENT, MS.DOBAVITELJ, MS.* FROM Grafolit55SI.dbo.MS MS  where MS.NAZIV = 'ROYALSET OFFSET 80G 64X90 SB'
SELECT MS.IDENT, MS.DOBAVITELJ, MS.* FROM Grafolit55SI.dbo.MS MS  where MS.NAZIV like '%SIRIO PEARL POLAR %'

delete from DW.dbo.DIM_Identi  where IDENT = '0712080301047   '


SELECT MS.IDENT AS StArtikla, MS.NAZIV, MS.DOBAVITELJ
FROM Grafolit55SI.dbo.MS MS
WHERE MS.DOBAVITELJ like '%SAPPI%'
AND MS.NAZIV LIKE '%64X90%'

update Povprasevanje set StatusID = 6 where PovprasevanjeID = 825
select * from Povprasevanje where Year(ts) = 2020
select * from Povprasevanje  order by 1 desc
select * from Povprasevanje where PovprasevanjeID = 11
select * from PovprasevanjePozicija where PovprasevanjeID = 11
select * from PovprasevanjePozicijaArtikel where PovprasevanjePozicijaID in (select PovprasevanjePozicijaID from PovprasevanjePozicija where PovprasevanjeID = 11)

update PovprasevanjePozicijaArtikel set IzbranArtikel = 0 where PovprasevanjePozicijaID in (select PovprasevanjePozicijaID from PovprasevanjePozicija where PovprasevanjeID = 11) and IzbranArtikel = 1
delete from Povprasevanje where PovprasevanjeID = 11
select * from StatusPovprasevanja

update SystemEmailMessage_PDO set EmailTo = TOEmails where len(EmailTo) = 0

select * from SystemEmailMessage_PDO order by 1 desc
update SystemEmailMessage_PDO set Status = 0 where SystemEmailMessageID in (136,140,138)
update SystemEmailMessage_PDO set Status = 1 where SystemEmailMessageID < 194
update SystemEmailMessage_PDO set Status = 0 where SystemEmailMessageID > 194

update KontaktnaOseba_PDO set Email = 'boris.dolinsek@gmail.com'

select * from GetArtikelByName('ARCTIC', '100x70')
select * from Grafolit55SI.dbo.MS MS
SELECT MS.IDENT, MS.DOBAVITELJ, MS.* FROM Grafolit55SI.dbo.MS MS  where MS.DOBAVITELJ like '%FEDRI%'

SELECT MS.IDENT, MS.DOBAVITELJ, MS.* FROM Grafolit55SI.dbo.MS MS  where NAZIV like '%130G 72X102%' and len(MS.DOBAVITELJ)>0 order by MS.DOBAVITELJ

--update StatusPovprasevanja set Koda = 'POSLANO_V_NABAVO', Naziv = 'Naroèilo poslano v nabavo', Opis = 'Naroèilo poslano v nabavo' where Koda = 'KREIRAN_POSLAN_PDF'
--update StatusPovprasevanja set Koda = 'POSLANO_V_NABAVO', Naziv = 'Naroèilo poslano v nabavo', Opis = 'Naroèilo poslano v nabavo' where Koda = 'KREIRAN_POSLAN_PDF'

Update SystemEmailMessage_PDO set EmailTo = 'boris.dolinsek@gmail.com',Status = 1
Update SystemEmailMessage_OTP set Status = 1

select * from StatusPovprasevanja
select * from SystemEmailMessage_PDO order by 1 desc

SELECT * FROM Grafolit55SI.dbo.tHE_Order AS G
      JOIN Grafolit55SI.dbo.tHE_OrderItem AS P
            ON G.acKey = P.acKey
WHERE
      G.acDocType IN ('0250') AND (G.acStatus IN (' ', '1', '2')) AND (G.adDate >= '20150101')
  order by G.adTimeIns desc

SELECT * FROM Grafolit55SI.dbo.tHE_Order AS G
WHERE
      G.acDocType IN ('0250') 
	  order by adTimeIns desc

select DISTINCT Kategorija from DW.dbo.DIM_Identi order by Kategorija desc

select * from DW.dbo.DIM_Identi order by Kategorija
select * from Grafolit55SI.dbo.MS where MS.DOBAVITELJ like'%SAPPI%' and MS.NAZIV like '%ROYALCOAT PREMIUM GLOSS 135G 64X90 SB%'

SELECT I.IDENT AS StArtikla, MS.NAZIV, Kategorija, Gloss, Gramatura, Velikost, Tek, DOBAVITELJ
FROM Grafolit55SI.dbo.MS MS
	JOIN DW.dbo.DIM_Identi I
		ON MS.IDENT = I.IDENT AND MS.DOBAVITELJ like '%SAPPI%'
WHERE	I.NAZIV LIKE '%61X86%'
		-- PREMAZ MATT
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'MATT', 'SILK') + '%'
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'MATT', 'SATIN') + '%'
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'MATT', 'MAT') + '%'
		-- PREMAZ GLOSS
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'GLOSS', 'GLOS') + '%'
		-- OFFSET
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'OFFSET', 'OFFSET AMBER') + '%'
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'OFFSET', 'OFFSET PRINT') + '%'
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'OFFSET', 'ARCOSET EXTRA WHITE') + '%'
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'OFFSET', 'ROYALSET EXTRA WHITE') + '%'
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'OFFSET', 'OFFSET PREPRINT') + '%'
		OR I.NAZIV LIKE '%' + REPLACE('AKIR MATT 70G', 'OFFSET', 'OFFSET SELENA') + '%'

SELECT UPPER(RTRIM(acName) + ' ' + RTRIM(acSurname)) as acSubject, acUserId, anUserID
FROM Grafolit55SI.dbo.tHE_SetSubjContact
WHERE acUserId IS NOT NULL

select * from SystemEmailMessage_PDO order by 1 desc

select * from KontaktnaOseba_PDO where Naziv = 'Aljoša Staniè'

select * from KontaktnaOseba_PDO order by 1 desc

select * from GetArtikelBySearchStr ('Kabel Premium Pulp & Paper Gmbh')