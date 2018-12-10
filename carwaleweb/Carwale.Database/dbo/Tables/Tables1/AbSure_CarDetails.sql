CREATE TABLE [dbo].[AbSure_CarDetails] (
    [Id]                                 NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [Make]                               VARCHAR (100)  NULL,
    [Model]                              VARCHAR (100)  NULL,
    [Version]                            VARCHAR (100)  NULL,
    [VersionId]                          INT            NULL,
    [VIN]                                VARCHAR (50)   NULL,
    [RegNumber]                          VARCHAR (50)   NULL,
    [EntryDate]                          DATETIME       CONSTRAINT [DF_AbSure_CarDetails_EntryDate] DEFAULT (getdate()) NULL,
    [Source]                             SMALLINT       NULL,
    [DealerId]                           BIGINT         NOT NULL,
    [StockId]                            BIGINT         NULL,
    [MakeYear]                           DATETIME       NULL,
    [Kilometer]                          INT            NULL,
    [Owners]                             VARCHAR (50)   NULL,
    [Colour]                             VARCHAR (50)   NULL,
    [Insurance]                          VARCHAR (50)   NULL,
    [InsuranceExpiry]                    DATETIME       NULL,
    [IsOrigionalRC]                      BIT            NULL,
    [IsBankHypothecation]                BIT            NULL,
    [RegisteredAt]                       VARCHAR (50)   NULL,
    [AvailableAt]                        VARCHAR (50)   NULL,
    [FuelType]                           SMALLINT       NULL,
    [Transmission]                       SMALLINT       NULL,
    [RegistrationDate]                   DATETIME       NULL,
    [RegistrationType]                   SMALLINT       NULL,
    [IsRejected]                         BIT            NULL,
    [IsSurveyDone]                       BIT            NULL,
    [RejectedDateTime]                   DATETIME       NULL,
    [ModifiedDate]                       DATETIME       NULL,
    [ImgLargeUrl]                        VARCHAR (250)  NULL,
    [ImgThumbUrl]                        VARCHAR (250)  NULL,
    [CarScore]                           INT            NULL,
    [AbSure_WarrantyTypesId]             INT            NULL,
    [FinalWarrantyTypeId]                INT            NULL,
    [WarrantyGivenBy]                    BIGINT         NULL,
    [FinalWarrantyDate]                  DATETIME       NULL,
    [SurveyDate]                         DATETIME       NULL,
    [IsSoldOut]                          BIT            CONSTRAINT [DF__AbSure_Ca__IsSol__1E96A5FE] DEFAULT ((0)) NULL,
    [OwnerName]                          VARCHAR (50)   NULL,
    [OwnerAddress]                       VARCHAR (500)  NULL,
    [OwnerPhoneNo]                       VARCHAR (50)   NULL,
    [OwnerEmail]                         VARCHAR (150)  NULL,
    [OwnerCityId]                        INT            NULL,
    [OwnerAreaId]                        INT            NULL,
    [Status]                             TINYINT        NULL,
    [CancelReason]                       VARCHAR (1000) NULL,
    [CancelledBy]                        BIGINT         NULL,
    [CancelledOn]                        DATETIME       NULL,
    [CarFittedWith]                      TINYINT        NULL,
    [ReqWarranty]                        INT            NULL,
    [EngineNo]                           VARCHAR (50)   NULL,
    [IsActive]                           BIT            CONSTRAINT [DF__AbSure_Ca__IsAct__68C4A532] DEFAULT ((1)) NULL,
    [AppointmentDate]                    DATETIME       NULL,
    [PolicyNo]                           VARCHAR (50)   NULL,
    [AppointmentTime]                    VARCHAR (50)   NULL,
    [AdId]                               VARCHAR (50)   NULL,
    [IsCancelled]                        BIT            NULL,
    [AbSureStatus]                       AS             ([dbo].[getAbsureStatus]([IsSurveyDone],[IsRejected],[FinalWarrantyDate],[Status])),
    [IsInspectionRescheduled]            BIT            NULL,
    [IsTestDrive]                        BIT            NULL,
    [RejectionMethod]                    INT            NULL,
    [PhotoCount]                         INT            NULL,
    [RejectionComments]                  VARCHAR (500)  NULL,
    [RCImagePending]                     BIT            NULL,
    [SurveySubmitMode]                   SMALLINT       NULL,
    [OnHoldComments]                     VARCHAR (500)  NULL,
    [CarSourceId]                        INT            NULL,
    [AbSureWarrantyActivationStatusesId] INT            NULL,
    CONSTRAINT [PK_AbSure_CarScore_CarDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AbSure_CarDetails_DealerId_]
    ON [dbo].[AbSure_CarDetails]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_AbSure_CarDetails_status]
    ON [dbo].[AbSure_CarDetails]([Status] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AbSure_CarDetails_EntryDate]
    ON [dbo].[AbSure_CarDetails]([EntryDate] ASC)
    INCLUDE([Id], [VersionId], [DealerId]);


GO
CREATE NONCLUSTERED INDEX [IX_AbSure_CarDetails_StockId]
    ON [dbo].[AbSure_CarDetails]([StockId] ASC);


GO
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 23rd Mar 2015
-- Description:	To generate policy No in case of absure.in when the final warranty field gets updated
-- Modified By : Ashwini Dhamankar on July 29,2105 , Inserted data into AbSure_CarsWithoutRCImage 
-- =============================================
CREATE TRIGGER [dbo].[TrigUpdateFinalWarranty]
   ON  [dbo].[AbSure_CarDetails]
   FOR UPDATE
AS 
BEGIN
	DECLARE @CarId INT,
			@FinalWarrantyTypeId INT, 
			@Status TINYINT,
			@PreStatus TINYINT,
			@IsRejected BIT,
			@IsCancelled BIT,
			@IsSurveyDone BIT,
			@DealerId INT,
			@StockId BIGINT,
			@RCNotMandatory BIT,
			@AbSure_CarPhotosId INT,
			@RCImagePending BIT

	SELECT	@CarId=I.Id, @FinalWarrantyTypeId = FinalWarrantyTypeId
	FROM	Inserted AS I
			INNER JOIN Absure_WarrantyInquiries WI ON WI.AbsureCarId = I.Id
	
	IF UPDATE(FinalWarrantyTypeId) AND @CarId IS NOT NULL
		EXEC Absure_UpdateWarrantyPolicyNumber @CarId,0 -- 0 is for absure.in


	/* Update Status Field in Absure_CarDetails Page
		0 or NULL - Inspection initiated by dealer but no action till now from AXA
		1	SurveyDone  - IF UPDATE(IsSurveyDone) AND IsSurveyDone = 1 THEN 1 ELSE 0
		2	Rejected	- IF UPDATE(IsRejected) AND IsRejected = 1 THEN 2
		3	Cancelled	- IF UPDATE(IsCancelled) AND IsCancelled = 1 THEN 3
		4	Accepted	- IF UPDATE(FinalWarrantyType) AND FinalWarrantyType > 0 THEN 4
		5	Surveyor Assigned - Update Through SP where there is surveyor assignment	
		6	Agency Assigned	 - Update Through SP where there is agency assignment	
		7	Certificate Expired	- One Automated running SP, Update Status only if warranty has not been activated
		8	Warranty Activated - Call SP on Warranty activation Form.
	*/
		
	SELECT	@CarId=I.Id, @FinalWarrantyTypeId = I.FinalWarrantyTypeId, @IsSurveyDone = I.IsSurveyDone, 
			@IsRejected = I.IsRejected, @IsCancelled = I.IsCancelled,@DealerId = I.DealerId,@StockId = I.StockId
	FROM	Inserted AS I
	
	SELECT @PreStatus = Status FROM Deleted D
	
	IF UPDATE(IsSurveyDone) AND @IsSurveyDone = 1
	BEGIN
		EXEC	[AbSure_UpdateStatus] 
				@AbSure_CarDetailsId	= @CarId,
				@Status					= 1,
				@ModifiedBy				= -1,
				@PreviousStatus			= @PreStatus,
				@IsTriggerCall			= 1

		SELECT @RCNotMandatory =  RCNotMandatory 
		FROM Dealers D WITH(NOLOCK) 
		WHERE D.ID = @DealerId

		SELECT @RCImagePending =  ISNULL(RCImagePending,0) 
		FROM AbSure_CarDetails ACD WITH(NOLOCK) 
		WHERE ACD.ID = @CarId
	
		--SELECT @AbSure_CarPhotosId = AbSure_CarPhotosId FROM AbSure_CarPhotos WHERE  AbSure_CarDetailsId = @CarId AND ((ImageTagType = 2 AND ImageTagId = 1) OR ImageCaption like 'RC' )
		  
		IF (@RCImagePending = 1)
		BEGIN
			IF NOT EXISTS(SELECT Id FROM AbSure_CarsWithoutRCImage WITH(NOLOCK) WHERE AbSure_CarDetailsId = @CarId)
			BEGIN
				INSERT INTO AbSure_CarsWithoutRCImage(AbSure_CarDetailsId,DealerId,StockId) VALUES (@CarId,@DealerId,@StockId)
			END
		END
	END							
	IF UPDATE(IsRejected) AND @IsRejected = 1
		EXEC	[AbSure_UpdateStatus] 
				@AbSure_CarDetailsId	= @CarId,
				@Status					= 2,
				@ModifiedBy				= -1,
				@PreviousStatus			= @PreStatus,
				@IsTriggerCall			= 1

	IF UPDATE(IsCancelled) AND @IsCancelled = 1
		EXEC	[AbSure_UpdateStatus] 
				@AbSure_CarDetailsId	= @CarId,
				@Status					= 3,
				@ModifiedBy				= -1,
				@PreviousStatus			= @PreStatus,
				@IsTriggerCall			= 1

	IF UPDATE(FinalWarrantyTypeId) AND @FinalWarrantyTypeId > 0
		EXEC	[AbSure_UpdateStatus] 
				@AbSure_CarDetailsId	= @CarId,
				@Status					= 4,
				@ModifiedBy				= -1,
				@PreviousStatus			= @PreStatus,
				@IsTriggerCall			= 1
END
