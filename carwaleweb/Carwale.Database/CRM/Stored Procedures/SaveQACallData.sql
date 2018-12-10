IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[SaveQACallData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[SaveQACallData]
GO

	CREATE PROCEDURE [CRM].[SaveQACallData]
@WeekId					INT,
@MonitorDate			DATETIME,
@CallDate				DATETIME,
@UserId					NUMERIC(18, 0),
@RecId					NUMERIC(18, 0),
@CallType				NUMERIC(18, 0),
@CustomerName			VARCHAR(50),
@MobileNo				VARCHAR(15),
@MakeId					INT,
@ProcessId				INT,
@Feedback				VARCHAR(3000) = NULL,
@FeedbackBy				NUMERIC(18,0),
@CustId					NUMERIC(18,0),
@IsFatal				BIT = NULL,
@FatalType				INT = NULL,
@FatalNature			VARCHAR(250) = NULL,
@QARoleId				INT,
@NewQACallDataId		NUMERIC(18,0) OUTPUT


AS 
BEGIN
	SET @NewQACallDataId = -1
	UPDATE CRM.QACallData SET WeekId=@WeekId,MonitorDate=@MonitorDate,CallDate=@CallDate,UserId=@UserId,
	CallType=@CallType,CustomerName=@CustomerName,MobileNo=@MobileNo,MakeId=@MakeId,ProcessId=@ProcessId,
	Feedback=@Feedback,FeedbackBy=@FeedbackBy,IsFatal=@IsFatal,QARoleId=@QARoleId,TypeOfFatal=@FatalType,
	NatureOfFatal=@FatalNature WHERE RecId=@RecId
	
	IF @@ROWCOUNT=0
		BEGIN 
			INSERT INTO CRM.QACallData (WeekId,MonitorDate,CallDate,UserId,RecId,CallType,CustomerName,MobileNo,MakeId,ProcessId,Feedback,FeedbackBy,CustId,IsFatal,QARoleId,TypeOfFatal,NatureOfFatal) 
			VALUES (@WeekId,@MonitorDate,@CallDate,@UserId,@RecId,@CallType,@CustomerName,@MobileNo,@MakeId,@ProcessId,@Feedback,@FeedbackBy,@CustId,@IsFatal,@QARoleId,@FatalType,@FatalNature)
			SET @NewQACallDataId = SCOPE_IDENTITY()
		END
	ELSE
		SELECT @NewQACallDataId=Id FROM CRM.QACallData WHERE RecId=@RecId
END
