IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Split]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Split]
GO

	CREATE FUNCTION [dbo].[Split](@Id varchar(200), @ValType varchar(200), @Value varchar(2000), @Delimiter char(1))       
returns @temptable TABLE (Id Int, ValType Int, Value varchar(2000))       
as       
begin       
    declare @idx int       
    declare @slice varchar(8000)
    declare @IdTable table(
		ID Int IDENTITY NOT NULL,
		ItemId Int
    )
    declare @valTypeTable table(
		ID Int Identity Not Null,
		ValType Int
    )       
    declare @valueTable table(
		ID Int Identity Not Null,
		Value Varchar(2000)
    ) 
    -- For CfId  
    select @idx = 1       
        if len(@Id)<1 or @Id is null  return       
      
    while @idx!= 0       
    begin       
        set @idx = charindex(@Delimiter,@Id)       
        if @idx!=0       
            set @slice = left(@Id,@idx - 1)       
        else       
            set @slice = @Id       
          
        if(len(@slice)>0)  
            insert into @IdTable(ItemId) values(@slice)       
  
        set @Id = right(@Id,len(@Id) - @idx)       
        if len(@Id) = 0 break       
    end   
    
    -- For ValType
    select @idx = 1       
        if len(@ValType)<1 or @ValType is null  return       
      
    while @idx!= 0       
    begin       
        set @idx = charindex(@Delimiter,@ValType)       
        if @idx!=0       
            set @slice = left(@ValType,@idx - 1)       
        else       
            set @slice = @ValType       
          
        if(len(@slice)>0)  
            insert into @valTypeTable(ValType) values(@slice)       
  
        set @ValType = right(@ValType,len(@ValType) - @idx)       
        if len(@ValType) = 0 break       
    end 
    
    -- For Value
    select @idx = 1       
        if len(@Value)<1 or @Value is null  return       
      
    while @idx!= 0       
    begin       
        set @idx = charindex(@Delimiter,@Value)       
        if @idx!=0       
            set @slice = left(@Value,@idx - 1)       
        else       
            set @slice = @Value       
          
        if(len(@slice)>0)  
            insert into @valueTable(Value) values(@slice)       
  
        set @Value = right(@Value,len(@Value) - @idx)
        if len(@Value) = 0 break       
    end 
    
    Insert into @temptable(Id, ValType, Value)
		(
			Select ItemId, ValType, Value From @IdTable As idt, @valTypeTable As vtt, @valueTable As vt Where idt.ID = vtt.ID And idt.ID = vt.ID
		)
    
return       
end  