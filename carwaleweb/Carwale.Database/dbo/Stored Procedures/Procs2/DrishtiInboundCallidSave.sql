IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DrishtiInboundCallidSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DrishtiInboundCallidSave]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 24th April 2014
-- Description : To save,update drishti calls data and to save calls that are not processed/missed.
-- Module      : Drishti Inbound/Outbound system
-- =============================================
CREATE PROCEDURE [dbo].[DrishtiInboundCallidSave]

@CallId				VARCHAR(200) = NULL,
@DialedNo    		VARCHAR(20) = NULL,
@User_ID			INT = NULL,
@CallStartDate		DATETIME = NULL,
@CallEndDate		DATETIME = NULL,
@DisconnectedBy		VARCHAR(50) = NULL,
@RingingTime		INT = NULL,
@TalkTime			INT = NULL,
@TotalTime			INT = NULL,
@CallStatus			VARCHAR(50) = NULL,
@CallTypeId			TINYINT = NULL,
@SetUpTime			FLOAT = NULL,
@SystemDisposition  VARCHAR(20) = NULL,
@DomainNo           VARCHAR(20)

AS
BEGIN
	DECLARE @IsCallUpdated BIT = 0
	--Update Drishti Calls Data
	UPDATE DST.DrishtiCallDetailS 
		SET DialedNo = CASE WHEN @DialedNo IS NOT NULL THEN @DialedNo ELSE DialedNo END,
			User_ID = CASE WHEN @User_ID IS NOT NULL THEN @User_ID ELSE User_ID END,
			CallStartDate = CASE WHEN @CallStartDate IS NOT NULL THEN @CallStartDate ELSE CallStartDate END,
			CallEndDate = CASE WHEN @CallEndDate IS NOT NULL THEN @CallEndDate ELSE CallEndDate END, 
			DisconnectedBy = CASE WHEN @DisconnectedBy IS NOT NULL THEN @DisconnectedBy ELSE DisconnectedBy END, 
			RingingTime = CASE WHEN @RingingTime IS NOT NULL THEN @RingingTime ELSE RingingTime END, 
			TalkTime = CASE WHEN @TalkTime IS NOT NULL THEN @TalkTime ELSE TalkTime END , 
			TotalTime = CASE WHEN @TotalTime IS NOT NULL THEN @TotalTime ELSE TotalTime END, 
			CallStatus = CASE WHEN @CallStatus IS NOT NULL THEN @CallStatus ELSE CallStatus END, 
			CallTypeId = CASE WHEN @CallTypeId IS NOT NULL THEN @CallTypeId ELSE CallTypeId END, 
			SetUpTime = CASE WHEN @SetUpTime IS NOT NULL THEN @SetUpTime ELSE SetUpTime END,
			IsCallIdUpdated = CASE WHEN @SystemDisposition IS NOT NULL THEN 1 ELSE 0 END,
			SystemDisposition = CASE WHEN @SystemDisposition IS NOT NULL THEN @SystemDisposition ELSE SystemDisposition END ,
			DomainNumber = CASE WHEN @DomainNo IS NOT NULL THEN @DomainNo ELSE SystemDisposition END
		WHERE CallId = @CallId	

	IF(@@ROWCOUNT= 0)
	--SET IDENTITY_INSERT DrishtiCallDetailS ON
		BEGIN
			INSERT INTO DST.DrishtiCallDetailS(CallId,IsCallIdUpdated,DialedNo,CallTypeId)--, DialedNo, User_ID, CallStartDate, CallEndDate, DisconnectedBy, RingingTime, TalkTime, TotalTime, CallStatus, CallTypeId, SetUpTime
			VALUES (@CallId,@IsCallUpdated,@DialedNo,@CallTypeId)--,@DialedNo,@User_ID,@CallStartDate,@CallEndDate,@DisconnectedBy,@RingingTime,@TalkTime,@TotalTime,@CallStatus,@CallTypeId,@SetUpTime
		END
	--SET IDENTITY_INSERT DrishtiCallDetailS OFF
	ELSE
		BEGIN
			DECLARE @CallType TINYINT
			SELECT @CallType = CallTypeId FROM DST.DrishtiCallDetailS WHERE CallId = @CallId
			--Insert inbound calls that are not processed
			IF((@User_ID IS NULL OR @TalkTime IS NULL OR @TalkTime = 0) AND @CallType = 1)
				BEGIN
					INSERT INTO DST.DrishtiNotProcessedCalls(CallId,PhoneNo,CreatedOn)
					VALUES(@CallId,@DialedNo,GETDATE())
				END
		END
END