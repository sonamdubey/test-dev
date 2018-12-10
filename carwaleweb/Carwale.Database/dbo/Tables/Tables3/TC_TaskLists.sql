CREATE TABLE [dbo].[TC_TaskLists] (
    [TC_TaskListsId]             INT            IDENTITY (1, 1) NOT NULL,
    [TC_CallsId]                 INT            NULL,
    [TC_CallTypeId]              TINYINT        NULL,
    [LastCallComment]            VARCHAR (1000) NULL,
    [TC_NextActionId]            SMALLINT       NULL,
    [ScheduledOn]                DATETIME       NULL,
    [BranchId]                   INT            NULL,
    [UserId]                     INT            NULL,
    [CustomerId]                 INT            NULL,
    [CustomerName]               VARCHAR (250)  NULL,
    [CustomerMobile]             VARCHAR (20)   NULL,
    [CustomerEmail]              VARCHAR (150)  NULL,
    [TC_LeadId]                  INT            NULL,
    [TC_InquiriesLeadId]         INT            NULL,
    [TC_InquiryStatusId]         TINYINT        NULL,
    [TC_LeadInquiryTypeId]       TINYINT        NULL,
    [TC_LeadDispositionId]       SMALLINT       NULL,
    [LatestInquiryDate]          DATETIME       NULL,
    [InqSourceId]                TINYINT        NULL,
    [TC_InquiriesLeadCreateDate] DATETIME       NULL,
    [TC_LeadStageId]             TINYINT        NULL,
    [InterestedIn]               VARCHAR (300)  NULL,
    [ExchangeCar]                VARCHAR (300)  NULL,
    [RecordCreatedOn]            DATETIME       CONSTRAINT [DF_TC_TaskLi_RecordCreatedOn] DEFAULT (getdate()) NULL,
    [OrderDate]                  DATETIME       NULL,
    [InquirySourceName]          VARCHAR (250)  NULL,
    [InquiryTypeName]            VARCHAR (150)  NULL,
    [IsVerified]                 BIT            NULL,
    [BucketTypeId]               SMALLINT       NULL,
    [Eagerness]                  VARCHAR (50)   NULL,
    [Location]                   VARCHAR (150)  NULL,
    [Car]                        VARCHAR (350)  NULL,
    [LeadAge]                    SMALLINT       NULL,
    [AssignedTo]                 VARCHAR (150)  NULL,
    [TC_BusinessTypeId]          TINYINT        NULL,
    [TC_NextActionDate]          DATETIME       NULL,
    [RegistrationNumber]         VARCHAR (50)   NULL,
    [ExpiryDate]                 DATETIME       NULL
);


GO
CREATE CLUSTERED INDEX [ix_TC_TaskLists_TC_CallsId]
    ON [dbo].[TC_TaskLists]([TC_CallsId] ASC);


GO
CREATE NONCLUSTERED INDEX [TC_TaskLists_BranchId]
    ON [dbo].[TC_TaskLists]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [TC_TaskLists_ScheduledOn]
    ON [dbo].[TC_TaskLists]([ScheduledOn] ASC);


GO
CREATE NONCLUSTERED INDEX [TC_TaskLists_TC_UserId]
    ON [dbo].[TC_TaskLists]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_TaskLists]
    ON [dbo].[TC_TaskLists]([ScheduledOn] ASC, [UserId] ASC, [BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_TaskLists_BucketTypeId]
    ON [dbo].[TC_TaskLists]([BucketTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_TaskLists_TC_LeadId]
    ON [dbo].[TC_TaskLists]([TC_LeadId] ASC)
    INCLUDE([TC_CallsId], [TC_CallTypeId], [ScheduledOn]);

