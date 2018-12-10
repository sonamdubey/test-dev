IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ActivateSoldRSAPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ActivateSoldRSAPackages]
GO

	--Author: Tejashree Patil on 9 Dec 2014
--Description: Activate RSA packages, StartDate is 3 days later excluding sunday and EndDate is 1 year after.
--============================================================================================================

CREATE PROCEDURE [dbo].[TC_ActivateSoldRSAPackages]
AS
BEGIN
	UPDATE	TC_SoldRSAPackages
	SET		StartDate = CASE
							WHEN ((DATEPART(dw, EntryDate) + @@DATEFIRST) % 7) BETWEEN DATEPART(dw,EntryDate) AND DATEPART(dw,EntryDate+3)
							THEN  EntryDate + 3
							ELSE  EntryDate + 4
						END
	WHERE  StartDate IS NULL AND EndDate IS NULL AND IsAccepted IS NULL

	UPDATE	TC_SoldRSAPackages
	SET		EndDate =	StartDate + 
						CASE 
							WHEN DATEDIFF(DAY,DATEADD(MONTH,1,DATEADD(YEAR,DATEPART(YEAR,StartDate)-1900,0)),DATEADD(MONTH,2,DATEADD(YEAR,DATEPART(YEAR,StartDate)-1900,0)))=29 
							THEN 366 
							ELSE 365
						END
	WHERE	EndDate IS NULL AND IsAccepted IS NULL

	UPDATE  TC_SoldRSAPackages
	SET		IsAccepted = 1
	WHERE	CONVERT(DATE,StartDate) = CONVERT(DATE,GETDATE())
END