IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMLoadTargetCopyData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMLoadTargetCopyData]
GO

	-- =============================================
-- Author:		Vishal Srivastava
-- Create date: 26-10-2013
-- Description:	Loads Market Parameters
--			  : Load the Saved Market Parameter
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMLoadTargetCopyData]
@TC_TMDistributionPatternMasterId INT =NULL,
@IsDealer BIT
	
AS
BEGIN
	SET NOCOUNT ON;	
	IF @IsDealer = 1
		BEGIN
			SELECT SP.UserName, 
			D.Organization AS FromDealer, 
			D1.Organization AS ToDealer, 
			TCD.EntryDate, 
			TCD.TargetValue, 
			TCD.TC_TMDistributionPatternMasterId AS FirstId, 
			TCD.TC_TMTargetCopyDataId AS SecondId,
			DATENAME(MONTH,DATEADD(MONTH, TCD.StartMonth - 1, 0)) as [MonthName]   
			FROM  
			TC_TMTargetCopyData TCD WITH (NOLOCK)
			JOIN TC_SpecialUsers SP WITH (NOLOCK) ON TCD.TC_SpecialUsersId = SP.TC_SpecialUsersId
			JOIN Dealers D WITH (NOLOCK) ON D.ID = TCD.CopyFrom
			JOIN Dealers D1 WITH (NOLOCK)  ON D1.ID = TCD.CopyTo
			WHERE TCD.IsDealer = 1
			AND TCD.TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId
			ORDER BY TC_TMTargetCopyDataId DESC
		END
	ELSE
		BEGIN
			SELECT SP.UserName, 
			M.Name + ' ' + D.Name AS FromDealer, 
			M1.Name + ' ' + D1.Name AS ToDealer, 
			TCD.EntryDate, 
			TCD.TargetValue, 
			TCD.TC_TMDistributionPatternMasterId AS FirstId, 
			TCD.TC_TMTargetCopyDataId AS SecondId,
			DATENAME(MONTH,DATEADD(MONTH, TCD.StartMonth - 1, 0)) as [MonthName]   
			FROM  
			TC_TMTargetCopyData TCD  WITH (NOLOCK)
			JOIN TC_SpecialUsers SP WITH (NOLOCK) ON TCD.TC_SpecialUsersId = SP.TC_SpecialUsersId
			JOIN CarVersions D WITH (NOLOCK) ON D.ID = TCD.CopyFrom
			JOIN CarModels M WITH (NOLOCK) ON D.CarModelId = M.ID 
			JOIN CarVersions D1  WITH (NOLOCK) ON D1.ID = TCD.CopyTo
			JOIN CarModels M1 WITH (NOLOCK) ON D1.CarModelId = M1.ID 
			WHERE TCD.IsDealer = 0
			AND TCD.TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId
			ORDER BY TC_TMTargetCopyDataId DESC
		END
END
