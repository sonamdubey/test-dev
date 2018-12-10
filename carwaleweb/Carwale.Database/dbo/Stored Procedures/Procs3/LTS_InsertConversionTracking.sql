IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_InsertConversionTracking]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_InsertConversionTracking]
GO

	--Procedure is for ConverSion Tracking  
CREATE PROCEDURE [dbo].[LTS_InsertConversionTracking]      
(      
@StdId AS BIGINT,  
@Type AS BIGINT,  
@CustomerId AS BIGINT,  
@InquiryId AS BIGINT,  
@EntryDateTime AS DATETIME,  
@Tag AS VARCHAR(100),  
@IPAddress  AS VARCHAR(50)   
)      
AS      
BEGIN      
       
INSERT INTO LTS_ConversionTracking (StdId, Type, CustomerId, InquiryId, EntryDateTime, Tag, IPAddress)      
 VALUES(@StdId,@Type,@CustomerId,@InquiryId,@EntryDateTime,@Tag,@IPAddress)      
  
END    