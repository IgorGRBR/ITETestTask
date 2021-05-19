USE [ITETASK2]
GO

SELECT a.NTOV, b.KOL, b.CENA
FROM dbo.TOV a
INNER JOIN (
	SELECT c.KOL as KOL, c.CENA as CENA, c.KTOV as KTOV
	FROM dbo.DMS c
	WHERE c.NDOC IN (
		SELECT d.NDM
		FROM dbo.DMZ d
		WHERE d.DDM = '2014-05-01' --Replace this date!
	)
) b
ON a.KTOV = b.KTOV
GO


