IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarUSPData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarUSPData]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 10-09-2014
-- Description:	Saving Data in Table CRM_CarUSPData
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveCarUSPData]
@USPDataId INT,
@VersionId INT, 
@ParameterId INT,
@USPFeatures VARCHAR(100),
@USPAdvantages VARCHAR(100),
@USPBenefit VARCHAR(100),
@IsActive BIT,
@CreatedBy INT

AS
	BEGIN

	DECLARE @VersionIdTemp INT

		SELECT @VersionIdTemp = VersionId FROM CRM_CarUSPData WHERE Id = @USPDataId
		IF (@USPDataId = -1) OR (@VersionIdTemp != @VersionId)
		BEGIN
			INSERT INTO CRM_CarUSPData ( VersionId, CarUSPParameterId, Feature, Benefit, Advantage, IsActive, CreatedBy) 
			VALUES (@VersionId, @ParameterId, @USPFeatures, @USPBenefit, @USPAdvantages, @IsActive, @CreatedBy)	
		END		

		ELSE
		BEGIN
			UPDATE CRM_CarUSPData 
				SET Feature = @USPFeatures, Benefit = @USPBenefit, Advantage = @USPAdvantages,
				IsActive = @IsActive, CreatedBy = @CreatedBy
			WHERE Id = @USPDataId
		END			
		
END
