CREATE TABLE [dbo].[MM_SellerMobileMasking] (
    [MM_SellerMobileMaskingId] INT          IDENTITY (1, 1) NOT NULL,
    [ConsumerId]               NUMERIC (18) NOT NULL,
    [ConsumerType]             TINYINT      NOT NULL,
    [MaskingNumber]            VARCHAR (20) NOT NULL,
    [Mobile]                   VARCHAR (35) NULL,
    [CreatedOn]                DATETIME     CONSTRAINT [DF_MM_SellerMobileMasking_CreatedOn] DEFAULT (getdate()) NULL,
    [ProductTypeId]            SMALLINT     NULL,
    [NCDBrandId]               INT          NULL,
    [LastUpdatedOn]            DATETIME     NULL,
    [LastUpdatedBy]            INT          NULL,
    [OldMaskingNumber]         VARCHAR (20) NULL,
    [DealerType]               INT          NULL,
    [LeadCampaignId]           INT          NULL,
    [ApplicationId]            TINYINT      DEFAULT ((1)) NULL,
    [ServiceProvider]          INT          DEFAULT ((1)) NULL,
    [TC_InquirySourceId]       INT          NULL,
    [ExpiryDate]               DATETIME     NULL,
    CONSTRAINT [PK_MM_SellerMobileMasking] PRIMARY KEY CLUSTERED ([MM_SellerMobileMaskingId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_MM_SellerMobileMasking_LeadCampaignId]
    ON [dbo].[MM_SellerMobileMasking]([LeadCampaignId] ASC);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 04-06-2013
---Description: This trigger will maintain the logs for updataion and insertion in [MM_SellerMobileMasking] Table.
---Modifier : Ruchira Patil on 30th June 2014 9Added column DealerType)
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigMM_SellerMobileMaskingLogs]
   ON [dbo].[MM_SellerMobileMasking]
   FOR INSERT,UPDATE
AS 
BEGIN 

   	IF ((select COUNT(*) from Deleted) =0 )
	BEGIN 
		INSERT INTO  MM_SellerMobileMaskingLog
												   (MM_SellerMobileMaskingId,
													ConsumerId,
													ConsumerType,
													MaskingNumber,
													Mobile,
													ProductTypeId,
													NCDBrandId,
													ActionTakenOn,
													ActionTakenBy,
													Remarks,
													DealerType)
											SELECT  
													MM_SellerMobileMaskingId,
													ConsumerId,
													ConsumerType,
													MaskingNumber,
													Mobile,
													ProductTypeId,
													NCDBrandId,
													GETDATE(),
													LastUpdatedBy,
													'Record Inserted',
													DealerType
			                                  
											 FROM inserted      
	          


	END 
   ELSE 
	   BEGIN 
			 INSERT INTO  MM_SellerMobileMaskingLog
												   (MM_SellerMobileMaskingId,
													ConsumerId,
													ConsumerType,
													MaskingNumber,
													Mobile,
													ProductTypeId,
													NCDBrandId,
													ActionTakenOn,
													ActionTakenBy,
													Remarks,
													DealerType)
											SELECT  
													MM_SellerMobileMaskingId,
													ConsumerId,
													ConsumerType,
													MaskingNumber,
													Mobile,
													ProductTypeId,
													NCDBrandId,
													GETDATE(),
													LastUpdatedBy,
													'Record Updated',
													DealerType
			                                  
											 FROM DELETED         
	   
	   
	   END
   


END


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 04-06-2013
-- Modified by: Manish on 05-06-2013 Requirement change
---Description: This trigger will maintain the logs for  deletion  in [MM_SellerMobileMasking] Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigMM_SellerMobileMaskingDeleteLogs]
   ON [dbo].[MM_SellerMobileMasking]
   FOR Delete
AS 
BEGIN
		 
		  INSERT INTO  MM_SellerMobileMaskingLog
											   (MM_SellerMobileMaskingId,
												ConsumerId,
												ConsumerType,
												MaskingNumber,
												Mobile,
												ProductTypeId,
												NCDBrandId,
												CreatedOn,
												LastUpdatedBy,
												Remarks)
		                                SELECT  
						                        MM_SellerMobileMaskingId,
												ConsumerId,
												ConsumerType,
												MaskingNumber,
												Mobile,
												ProductTypeId,
												NCDBrandId,
												GETDATE(),
												LastUpdatedBy,
												'Record Deleted'
		                                  
		                                 FROM DELETED									 
 END

GO
DISABLE TRIGGER [dbo].[TrigMM_SellerMobileMaskingDeleteLogs]
    ON [dbo].[MM_SellerMobileMasking];

