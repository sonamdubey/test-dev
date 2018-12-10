IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertorUpdatePQDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertorUpdatePQDealers]
GO
	
-- ============================================= 
-- Author: Kumar Vikram    
-- Create date:07/03/2014
-- Description:for inserting and updating pq dealers data
-- Modified by Ruchira Patil on 3th July 2014 (added ZoneId parameter and made city,model,zone multiple and inserting them in PQ_DealerCitiesModels)
-- Modified by Ruchira Patil on 29th Sept 2014 (commented the delete query for PQ_DealerCitiesModels table)
-- Modified by Ruchira Patil on 31th Sept 2014 (added columns type,totalgoal,dailygoal)
-- Modified by Ruchira Patil on 16th Oct 2014 (added parameter leadPanel)
-- Modified by Ruchira Patil on 20th Oct 2014 (added parameter IsMail and IsSMS)
-- Modified by Ruchira Patil on 3rd Nov 2014 (added parameters campaignID and templateid. Call SP [SavePQCampaignTemplate] to save templates)
-- Modified By Khushaboo Patil on 23rd Feb 2015 (added parameter LinkText)
-- Modified By Vikas J on 23rd Feb 2015 (added parameter LinkText)
-- Modified By Sourav Roy on 11th Jun 2015 (Updating Total Count and daily Count on reset To PQ_DealerSponsored)
-- Modified By Sourav Roy on 11th Jun 2015 (Updating Last Total Count and last daily Count on reset To PQ_DealerSponsoredLog)
-- Modified varchar to datetime added 23:59:59 for campaign to last till end of day.Passed Isactive for update/Resume(Vinayak) 
-- and conditional insert to pq_dealersponsoredlogs
-- ============================================= 
CREATE PROCEDURE [dbo].[InsertorUpdatePQDealers]
-- Add the parameters for the stored procedure here 
@IsRule bit,
@Id int,
@MakeId int,
@ModelId varchar(max),
@StateId int,
@CityId varchar(max),
@ZoneId varchar(max),
@DealerId int,
@DealerName varchar(200),
@Phone varchar(50),
@DealerEmail varchar(250),
@StartDate varchar(50),
@EndDate varchar(50),
@UpdatedBy int,
@IsDesktop bit,
@IsMobile bit,
@IsAndroid bit,
@IsIPhone bit,
@Type int = NULL,
@TotalGoal int = NULL,
@DailyGoal int = NULL,
@LeadPanel tinyint = NULL,
@CampPriority int,
@TemplateId varchar(20),
@LinkText varchar(250) = NULL,
@Status int = NULL OUTPUT,
@NewId int = NULL OUTPUT,
@EnableUserEmail bit = NULL,--Modified By : Vikas J on 19/05/15 Added four new output parametes
@EnableUserSMS bit = NULL,
@EnableDealerEmail bit = NULL,
@EnableDealerSMS bit = NULL,
@CheckReset bit = NULL, --Added By : Sourav Roy on 11/06/15 Added one new parametes
@Remarks varchar(50) = NULL,--Added By : Sourav Roy on 11/06/15 Added three new parametes
@LastTotalCount int = NULL, 
@LastDailyCount int = NULL,
@CostPerLead decimal(7,2) = NULL,--Added By : Sourav Roy on 29/06/15
@IsActive bit
AS
BEGIN
  DECLARE @TempTblCity TABLE (
    ID int IDENTITY (1, 1),
    City int
  )
  DECLARE @TempTblModel TABLE (
    ID int IDENTITY (1, 1),
    Model int
  )

  DECLARE @TempCity int,
          @TempModel int,
          @TempMakeId int,
          @CityCounter int,
          @ModelCounter int,
          @i tinyint = 1,
          @j tinyint = 1,
		  @End_Date datetime,
		  @Start_Date datetime
	SET @End_Date=DATEADD(SS,59,DATEADD(MI,59,DATEADD(HH,23,convert(datetime,@EndDate,101))))--added 23:59:59 for campaign to last till end of day
	SET @Start_Date=CONVERT(datetime,@StartDate,101)
  IF @IsRule = 0
  BEGIN
    IF @Id = -1
    BEGIN
      INSERT INTO PQ_DealerSponsored (DealerId, DealerName, Phone, IsActive, DealerEmailId, StartDate, EndDate, UpdatedBy, UpdatedOn, IsDesktop, IsMobile, IsAndroid, IsIPhone, Type, TotalGoal, DailyGoal, LeadPanel, CampaignPriority, LinkText, EnableUserEmail, EnableUserSMS, EnableDealerEmail, EnableDealerSMS,CostPerLead)
        VALUES (@DealerId, @DealerName, @Phone, @IsActive, @DealerEmail, @Start_Date, @End_Date, @UpdatedBy, GETDATE(), @IsDesktop, @IsMobile, @IsAndroid, @IsIPhone, @Type, @TotalGoal, @DailyGoal, @LeadPanel, @CampPriority, @LinkText, @EnableUserEmail, @EnableUserSMS, @EnableDealerEmail, @EnableDealerSMS,@CostPerLead)
      
	  SET @NewId = SCOPE_IDENTITY()

      INSERT INTO PQ_DealerSponsoredLog (PQ_DealerSponsoredId, DealerId, DealerName, Phone, IsActive, DealerEmailId, StartDate, EndDate, ActionTakenBy, ActionTakenOn, IsDesktop, IsMobile, IsAndroid, IsIPhone, Type, TotalGoal, DailyGoal, LeadPanel, CampaignPriority, LinkText, Remarks, EnableUserEmail, EnableUserSMS, EnableDealerEmail, EnableDealerSMS,CostPerLead)
        VALUES (@NewId, @DealerId, @DealerName, @Phone, @IsActive, @DealerEmail, @Start_Date, @End_Date, @UpdatedBy, GETDATE(), @IsDesktop, @IsMobile, @IsAndroid, @IsIPhone, @Type, @TotalGoal, @DailyGoal, @LeadPanel, @CampPriority, @LinkText, 
		case when @IsActive=1 then 'Record Inserted'
		ELSE 'Record Removed' end
		, @EnableUserEmail, @EnableUserSMS, @EnableDealerEmail, @EnableDealerSMS,@CostPerLead)
    END
    ELSE
    BEGIN
      UPDATE PQ_DealerSponsored
      SET DealerName = @DealerName,
          DealerId = @DealerId,
          Phone = @Phone,
          DealerEmailId = @DealerEmail,
          StartDate = @Start_Date,
          EndDate = @End_Date,
          IsActive=@IsActive,
		  UpdatedBy = @UpdatedBy,
          UpdatedOn = GETDATE(),
          IsDesktop = @IsDesktop,
          IsMobile = @IsMobile,
          IsAndroid = @IsAndroid,
          IsIPhone = @IsIPhone,
          Type = @Type,
          TotalGoal = @TotalGoal,
          DailyGoal = @DailyGoal,
          LeadPanel = @LeadPanel,
          EnableUserEmail = @EnableUserEmail,
          EnableUserSMS = @EnableUserSMS,
          EnableDealerEmail = @EnableDealerEmail,
          EnableDealerSMS = @EnableDealerSMS,
          CampaignPriority = @CampPriority,
          LinkText = @LinkText,
		  TotalCount= CASE WHEN @CheckReset = 1 THEN 0 ELSE TotalCount END,
		  DailyCount=CASE WHEN @CheckReset = 1 THEN 0 ELSE DailyCount END,
		  CostPerLead=@CostPerLead
      WHERE Id = @Id
      SET @NewId = @Id

      INSERT INTO PQ_DealerSponsoredLog (PQ_DealerSponsoredId, DealerId, DealerName, Phone, IsActive, DealerEmailId, StartDate, EndDate, ActionTakenBy, ActionTakenOn, IsDesktop, IsMobile, IsAndroid, IsIPhone, Type, TotalGoal, DailyGoal, LeadPanel, CampaignPriority, LinkText, Remarks, EnableUserEmail, EnableUserSMS, EnableDealerEmail, EnableDealerSMS,LastTotalCount,LastDailyCount,CostPerLead)
        SELECT
          Id,
          DealerId,
          DealerName,
          Phone,
          IsActive,
          DealerEmailId,
          StartDate,
          EndDate,
          UpdatedBy,
          UpdatedOn,
          IsDesktop,
          IsMobile,
          IsAndroid,
          IsIPhone,
          Type,
          TotalGoal,
          DailyGoal,
          LeadPanel,
          @CampPriority,
          @LinkText,
          @Remarks,
          EnableUserEmail,
          EnableUserSMS,
          EnableDealerEmail,
          EnableDealerSMS,
		  @LastTotalCount,
		  @LastDailyCount,
		  @CostPerLead
        FROM PQ_DealerSponsored
        WHERE Id = @Id
    END

    EXEC SavePQCampaignTemplate @CampaignId = @NewId,
                                @TemplateId = @TemplateId
  END
  ELSE
  BEGIN
    INSERT INTO @TempTblCity (City)
      SELECT
        ListMember
      FROM fnSplitCSV(@CityId)
    INSERT INTO @TempTblModel (Model)
      SELECT
        ListMember
      FROM fnSplitCSV(@ModelId)

    --Updating the cities and models related to above offer

    --Delete Initial data for that particular CampaignId only if makeid is same
    --DELETE FROM PQ_DealerCitiesModels WHERE CampaignId=@Id AND MakeId = (SELECT DISTINCT MakeId FROM PQ_DealerCitiesModels WHERE CampaignId=@Id AND MakeId=@MakeId)

    SELECT
      @CityCounter = COUNT(ID)
    FROM @TempTblCity

    WHILE @CityCounter > 0
    BEGIN
      SELECT
        @TempCity = City
      FROM @TempTblCity
      WHERE ID = @i
      SET @i = @i + 1
      SET @CityCounter = @CityCounter - 1

      SELECT
        @ModelCounter = COUNT(ID)
      FROM @TempTblModel

      SET @j = 1

      WHILE @ModelCounter > 0
      BEGIN
        SELECT
          @TempModel = Model
        FROM @TempTblModel
        WHERE ID = @j
        SET @j = @j + 1
        SET @ModelCounter = @ModelCounter - 1

        IF @ZoneId IS NOT NULL
        BEGIN
          INSERT INTO PQ_DealerCitiesModels (PqId, CampaignId, CityId, ZoneId, DealerId, ModelId, StateId, MakeId)
            SELECT
              @Id,
              @Id,
              @TempCity,
              ListMember,
              @DealerId,
              @TempModel,
              @StateId,
              @MakeId
            FROM fnSplitCSV(@ZoneId)
        END
        ELSE
        BEGIN
          INSERT INTO PQ_DealerCitiesModels (PqId, CampaignId, CityId, ZoneId, DealerId, ModelId, StateId, MakeID)
            VALUES (@Id, @Id, @TempCity, NULL, @DealerId, @TempModel, @StateId, @MakeId)
        END
      END
    END
  END
  SET @Status = 1
END



/****** Object:  StoredProcedure [dbo].[Con_DeletePQdealers]    Script Date: 7/7/2015 12:13:51 PM ******/
SET ANSI_NULLS ON
