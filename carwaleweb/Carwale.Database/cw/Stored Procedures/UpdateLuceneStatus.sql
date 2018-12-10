IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[UpdateLuceneStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[UpdateLuceneStatus]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <10/22/2013>      
-- Description: <Updates the status of IsIndexed column.> 
-- =============================================      
create procedure [cw].[UpdateLuceneStatus]     
 -- Add the parameters for the stored procedure here      
 @threadid NUMERIC(18,0),
 @Status bit
AS      
BEGIN      
Update Forums SET IsIndexed = @Status Where ID = @threadid  
END 
