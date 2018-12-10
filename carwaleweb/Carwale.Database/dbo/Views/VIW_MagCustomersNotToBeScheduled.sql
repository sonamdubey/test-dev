IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'VIW_MagCustomersNotToBeScheduled' AND
     DROP VIEW dbo.VIW_MagCustomersNotToBeScheduled
GO

	Create View VIW_MagCustomersNotToBeScheduled AS
Select
CustomerId
From
MG_BlockedCustomers

UNION

Select
MR.CustomerId
From
MG_Requests AS MR,
MG_Volumes AS MV
Where
MV.CurrentVolume = 1 AND
MV.VolumeId = MR.VolumeId AND 
MR.DeliveryStatus = -1

UNION

Select
MS.CustomerId
From
MG_Scheduled AS MS,
MG_Volumes AS MV
Where
MV.CurrentVolume = 1 AND
MS.VolumeId = MV.VolumeId
