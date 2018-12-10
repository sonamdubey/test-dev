IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CallInserted]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CallInserted]
GO

	-- =============================================  
-- Author:  Manish  
-- Create date: 09-Jan-12  
-- Description: Inserting calls when call cross verification stage
-- Modified By Deepak on 28th Sep 2016
-- =============================================  
CREATE PROCEDURE [dbo].[TC_CallInserted] @TC_CallsId       AS INT, 
                                        @TC_UsersId       AS INT, 
                                        @Comment          AS VARCHAR(max), 
                                        @NextFolloupDate  AS DATETIME,
                                        @CallType         AS TINYINT =3   ----Call Type New Lead alert since lead crossing verification stage.
                                         
AS 
	BEGIN 
		DECLARE @TC_LeadId	INT, @AlertId INT
		DECLARE @ScheduleDate DATETIME
		
		SET @ScheduleDate = ISNULL(@NextFolloupDate,GETDATE())
		
		SELECT @TC_LeadId = TC_LeadId, @AlertId = AlertId
		FROM   TC_Calls WITH (NOLOCK)
		WHERE  TC_CallsId = @TC_CallsId  
			
		EXEC TC_ScheduleCall @TC_UsersId, @TC_LeadId, @CallType, @ScheduleDate, @AlertId, @Comment, NULL , NULL, NULL, NULL, NULL

    END 
 
