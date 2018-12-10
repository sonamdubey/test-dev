IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_LogRequestUrl_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_LogRequestUrl_SP]
GO

	


CREATE  PROCEDURE [dbo].[CH_LogRequestUrl_SP]
	@RequestCallId		NUMERIC,
	@RequestUrl		VARCHAR(100),
	@RequestDateTime	DateTime
	
AS
	
BEGIN
	INSERT INTO CH_LogRequestUrl
		(
			RequestCallId,	RequestUrl,	RequestDateTime
		)	
	VALUES
		(
			@RequestCallId,	@RequestUrl,	@RequestDateTime
		)	
	
END

