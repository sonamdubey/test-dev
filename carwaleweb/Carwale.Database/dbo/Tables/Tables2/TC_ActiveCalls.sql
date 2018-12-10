CREATE TABLE [dbo].[TC_ActiveCalls] (
    [TC_CallsId]             INT           NOT NULL,
    [TC_LeadId]              INT           NULL,
    [CallType]               TINYINT       NOT NULL,
    [TC_UsersId]             INT           NOT NULL,
    [ScheduledOn]            DATETIME      NOT NULL,
    [AlertId]                INT           NULL,
    [LastCallDate]           DATETIME      NULL,
    [LastCallComment]        VARCHAR (MAX) NULL,
    [TC_NextActionId]        SMALLINT      NULL,
    [TC_ActionApplicationId] INT           NULL,
    [NextCallTo]             INT           NULL,
    [TC_BusinessTypeId]      TINYINT       NULL,
    [TC_NextActionDate]      DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([TC_CallsId] ASC),
    CONSTRAINT [DF_Tc_ActiveCalls_TC_CallType] FOREIGN KEY ([CallType]) REFERENCES [dbo].[TC_CallType] ([TC_CallTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ActiveCalls_TC_UsersId]
    ON [dbo].[TC_ActiveCalls]([TC_UsersId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_ActiveCalls_TC_LeadId]
    ON [dbo].[TC_ActiveCalls]([TC_LeadId] DESC);


GO

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <11 Dec 15>
-- Description:	<Insert data into Notification to notify user if new lead is inserted or lead is transfered>
-- =============================================
CREATE TRIGGER [dbo].[TrigSaveNotifications]
   ON  [dbo].[TC_ActiveCalls]
   AFTER  INSERT,UPDATE
AS 
BEGIN
	DECLARE @CallType INT
	SELECT @CallType = CallType FROM inserted
	
	DECLARE @type CHAR(1)=
    CASE WHEN NOT EXISTS(SELECT * FROM INSERTED)
        THEN 'D'
    WHEN EXISTS(SELECT * FROM DELETED)
        THEN 'U'
    ELSE
        'I'
    END

	DECLARE @TC_UserId INT,
	@RecordId	INT
	SELECT @TC_UserId = TC_UsersId , @RecordId = TC_CallsId FROM inserted
	IF @CallType = 1 AND @type = 'I'		
		EXEC TC_WebNotificationSave @TC_UserId,@RecordId,1
		--INSERT INTO TC_Notifications(TC_UserId,RecordId,RecordType,NotificationDateTime)
		--SELECT TC_UsersId,TC_CallsId,1,GETDATE() FROM inserted

	ELSE IF(UPDATE(TC_UsersId)) AND @type = 'U'
	BEGIN
		EXEC TC_WebNotificationSave @TC_UserId,@RecordId,2
		--INSERT INTO TC_Notifications(TC_UserId,RecordId,RecordType,NotificationDateTime)
		--SELECT TC_UsersId,TC_CallsId,2,GETDATE() FROM inserted
	END
END



---------------------------------------------------------------------------------------------------------------------------------------------------

