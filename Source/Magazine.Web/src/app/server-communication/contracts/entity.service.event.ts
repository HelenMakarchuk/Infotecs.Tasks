import { EntityService } from "../../contracts/service/entity.service";
import { Serializable, jsonIgnore, jsonName } from "ts-serializable";

/** Событие сервиса сущности. */
export abstract class EntityServiceEvent extends Serializable {
    
    @jsonIgnore()
    protected serverServiceEventClassAssembly = "Infotecs.Magazine.API";
    
    @jsonIgnore()
    protected serverServiceEventClassNamespace = "Magazine.API.ClientCommunicationService.Events";

    @jsonName("$type")
    serverFullClassName = "";

    className = "";

    constructor(className: string) {
        super();
        this.className = className;
        this.serverFullClassName = `${this.serverServiceEventClassNamespace}.${this.className}, ${this.serverServiceEventClassAssembly}`;
    }

    abstract visit(component: EntityService): void;
}
