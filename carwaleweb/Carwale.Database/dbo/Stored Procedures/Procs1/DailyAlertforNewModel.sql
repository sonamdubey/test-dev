IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DailyAlertforNewModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DailyAlertforNewModel]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 08-11-2013
-- Description:	Daily alert if new version created during the day.
-- Modified by: Manish on 25-03-2014 for New Model addition alert as per Bharati Axa requirement.
-- Modified by: Manish on 05-08-2015 alert will go only if column name  New value is change from 0 to 1
-- Modified by: Manish on 06-08-2015  date condition changed considered time of the day as well 
-- =============================================
CREATE PROCEDURE [dbo].[DailyAlertforNewModel]	
AS
	BEGIN

	/*	SELECT M.id as CarMake, M.Name as Make,CM.id as ModelId,CM.Name as ModelName, CONVERT(DATE,L.CreatedOn) as CreatedOn
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN carModels as CM on CM.ID=L.AffectedId
		JOIN CarMakes as M on M.Id=CM.Carmakeid
		WHERE Tablename='CarModels' 
		AND Remarks='Record Inserted'
		AND CONVERT(DATE,L.CreatedOn)>=CONVERT(DATE,GETDATE()-1)
		ORDER BY L.CreatedOn DESC  */

	SELECT M.id as CarMake, M.Name as Make,CM.id as ModelId,CM.Name as ModelName, CONVERT(DATE,L.CreatedOn) as LaunchedOn
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN carModels as CM  WITH (NOLOCK) on CM.ID=L.AffectedId
		JOIN CarMakes as M  WITH (NOLOCK) on M.Id=CM.Carmakeid
		WHERE L.Tablename='CarModels' 
		AND   L.Remarks='Record Inserted'
		AND   CM.New=1
		AND   L.CreatedOn>=(GETDATE()-1)     --CONVERT(DATE,GETDATE()-1)
  UNION 
	SELECT M.id as CarMake, M.Name as Make,CM.id as ModelId,CM.Name as ModelName, CONVERT(DATE,L.CreatedOn) as LaunchedOn
		FROM CarWaleMasterDataLogs AS L WITH (NOLOCK)
		JOIN carModels as CM  WITH (NOLOCK) on CM.ID=L.AffectedId
		JOIN CarMakes as M WITH (NOLOCK)  on M.Id=CM.Carmakeid
		WHERE L.Tablename='CarModels' 
		AND   L.Remarks='Record Updated'
		AND   L.ColumnName='NEW'
		AND   L.OldValue=0
		AND   L.NewValue=1
		AND   L.CreatedOn>=(GETDATE()-1) --CONVERT(DATE,GETDATE()-1)
		ORDER BY LaunchedOn DESC
	END
