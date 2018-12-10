IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Impact_AutoRemoveCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Impact_AutoRemoveCampaigns]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 20th Oct 2014
-- Description : Automatic Update Impact_Slot And Impact_Campaign
-- =============================================

CREATE PROCEDURE [dbo].[Impact_AutoRemoveCampaigns]
   
 AS
 
	DECLARE 
	@ImpactCampaignId    INT,
	@MakeId              INT,
    @CityId              INT,
    @PackageId           INT ,
    @DealerID            INT,
	@IsActive            BIT = 0,
	@Action              VARCHAR(50) = 'Deactivate',
	@StartDate           DateTime,
	@EndDate             DateTime,
	@UpdatedBy           INT = 13,
	@ReturnStatus        BIT 

    BEGIN 
		DECLARE @TempTable TABLE 
			(
			    TempTableId         INT NOT NULL IDENTITY (1, 1),
				ImpactCampaignId    INT,
				MakeId              INT,
				CityId              INT,
				PackageId           INT ,
				DealerID            INT,
				StartDate           DateTime,
				EndDate             DateTime
			)
        DECLARE @RowsCount INT = 0

		INSERT INTO @TempTable
			SELECT IMC.Impact_CampaignId,IMC.MakeId,IMC.CityId ,IMC.PackageTypeId,IMC.DealerId,IMC.StartDate,IMC.EndDate 
			FROM Impact_Campaign AS IMC WITH(NOLOCK)
			WHERE EndDate < GETDATE() AND IMC.IsActive=1

		SELECT  @RowsCount = COUNT(ImpactCampaignId) FROM @TempTable
				
		WHILE @RowsCount<>0
			BEGIN

				SELECT @ImpactCampaignId=TT.ImpactCampaignId,@MakeId=TT.MakeId,@CityId=TT.CityId,@PackageId=TT.PackageId,@DealerID = TT.DealerID,
					@StartDate= TT.StartDate,@EndDate=TT.EndDate
				FROM @TempTable AS TT WHERE TempTableId=@RowsCount
					
				EXEC Impact_CampaignUpdate 
									@ImpactCampaignId,
									@MakeId,
									@CityId,
									@PackageId,
									@DealerID,
									@IsActive,
									@Action,
									@StartDate,
									@EndDate,
									@UpdatedBy,
									@ReturnStatus
				SET @RowsCount = @RowsCount-1
	      END

   END