import { ConfigService } from './config.service';
import { Config } from './config';

export class ConfigComponent {

  config: Config;
  headers: string[];

  constructor(private configService: ConfigService) { }
}
